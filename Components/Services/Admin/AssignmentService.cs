using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class AssignmentService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly ClassSessionService _sessionService;

    public AssignmentService(IDbContextFactory<ApplicationDbContext> context, ClassSessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    // GET ALL ASSIGNMENTS BY CLASSSESSION
    public async Task<List<Assignments>> GetAllAssignmentsByClassSession(int classSessionId, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await AssignmentsQuery(context)
                .Where(x => x.SessionId == classSessionId)
                .ToListAsync();
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }

    }
    // GET ASSIGNMENT BY ID
    public async Task<Assignments> GetAssignmentById(int assignmentId, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await AssignmentsQuery(context)
                .Where(x => x.Id == assignmentId)
                .FirstAsync();
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }

    }
    // GET ASSIGNMENT BY STUDENT
    public async Task<List<Grade>> GetAllAssignmentsByStudentAndClassSession(Student student, int classSessionId, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.GradeDb
                .Where(x => x.SessionId == classSessionId && x.StudentProgramId == student.MyProgram.Id)
                .Include(x => x.StudentProgram)
                    .ThenInclude(x => x.User)
                .Include(x => x.Assignment)
                .Include(x => x.Session)
                    .ThenInclude(x => x.Course)
                .ToListAsync();
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }

    // CREATE NEW ASSIGNMENT BY CLASS SESSION
    public async Task<Assignments> CreateAssignmentByClassSession(Assignments assignment, int classSessionId, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existingSession = await _sessionService.GetClassSessionById(classSessionId, context);
            if (existingSession == null) throw new Exception("No existing session found to add an assignment to.");

            Assignments entity = new()
            {
                Name = assignment.Name,
                TotalPoints = assignment.TotalPoints,
                SessionId = classSessionId,
                Instructions = assignment.Instructions,
            };

            // existingSession.Assignments.Add(assignment); // Add this assignment to the class session

            context.AssignmentsDb.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }

    }

    // UPDATE ASSIGNMENT 
    public async Task<Assignments> UpdateAssignment(Assignments updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existing = await GetAssignmentById(updated.Id, context);
            if (existing == null) throw new Exception("No existing assignment found to update.");

            existing.Name = updated.Name;
            existing.TotalPoints = updated.TotalPoints;
            existing.Instructions = updated.Instructions;
            existing.SessionId = updated.SessionId;
            existing.Grades = updated.Grades;

            await context.SaveChangesAsync();
            return existing;
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }

    }
    // DELETE ASSIGNMENT | Parameters (assignmentId , database context)
    public async Task DeleteAssignment(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            await context.AssignmentsDb.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }

    // GRADE Student copy of Assignment | Save to Student account, save class sesison ID in grade, save to assignment grades
    public async Task SaveGrades(List<Grade> grades, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            foreach(Grade grade in grades) {
                if (grade.ScoredPoints >= 0) throw new ValidationException("Scores cannot be negative.");
            }

            var ids = grades
                .Select(x => x.Id)
                .ToList();

            var existingGrades = await context.GradeDb
                .Where(g => ids.Contains(g.Id))
                .ToDictionaryAsync(g => g.Id);

            foreach (var grade in grades)
            {
                if (!existingGrades.TryGetValue(grade.Id, out var existing))
                    throw new Exception($"Could not find grade {grade.Id}.");

                existing.ScoredPoints = grade.ScoredPoints;
            }

            await context.SaveChangesAsync();

        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }

    // Standard Assignment Query
    private IQueryable<Assignments> AssignmentsQuery(ApplicationDbContext context)
    {
        return context.AssignmentsDb
            .Include(x => x.Session)
                .ThenInclude(x => x!.Course)
            .Include(x => x.Grades)
                .ThenInclude(x => x.StudentProgram)
                    .ThenInclude(x => x.User);
    }


}