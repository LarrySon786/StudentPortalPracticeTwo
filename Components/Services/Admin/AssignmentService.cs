using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class AssignmentService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context; // Replaced by _createDispose which creates the context
    private readonly ClassSessionService _sessionService;
    private readonly CreateDisposeContextHelper _createDispose;

    public AssignmentService(IDbContextFactory<ApplicationDbContext> context, ClassSessionService sessionService, CreateDisposeContextHelper createDispose)
    {
        _context = context;
        _sessionService = sessionService;
        _createDispose = createDispose;
    }

    // GET ALL ASSIGNMENTS BY CLASSSESSION
    public async Task<List<Assignments>> GetAllAssignmentsByClassSession(int classSessionId, ApplicationDbContext? context = null)
    {

        return await _createDispose.ExecuteAsync(db => AssignmentsQuery(db)
                .Where(x => x.SessionId == classSessionId)
                .ToListAsync()
        , context);
        

    }
    // GET ASSIGNMENT BY ID
    public async Task<Assignments> GetAssignmentById(int assignmentId, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => AssignmentsQuery(db) // Pass search query in
                .Where(x => x.Id == assignmentId) // Search query
                .FirstAsync(),
            context); // Pass context in
    }
    // GET ASSIGNMENT BY STUDENT
    public async Task<List<Grade>> GetAllAssignmentsByStudentAndClassSession(Student student, int classSessionId, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.GradeDb
                .Where(x => x.SessionId == classSessionId && x.StudentProgramId == student.MyProgram.Id)
                .Include(x => x.StudentProgram)
                    .ThenInclude(x => x.User)
                .Include(x => x.Assignment)
                .Include(x => x.Session)
                    .ThenInclude(x => x.Course)
                .ToListAsync()
            , context);

        
        
    }

    // CREATE NEW ASSIGNMENT BY CLASS SESSION
    public async Task<Assignments> CreateAssignmentByClassSession(Assignments assignment, int classSessionId, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var existingSession = await _sessionService.GetClassSessionById(classSessionId, db);
            if (existingSession == null) throw new Exception("No existing session found to add an assignment to.");

            Assignments entity = new()
            {
                Name = assignment.Name,
                TotalPoints = assignment.TotalPoints,
                SessionId = classSessionId,
                Instructions = assignment.Instructions,
                Grades = [],
            };

            foreach (UserProgramModel studentProgram in existingSession.StudentProgramModels)
            {
                Grade newGrade = new()
                {
                    SessionId = classSessionId,
                    Assignment = entity,
                    StudentProgramId = studentProgram.Id,
                    Submitted = false,
                };
                studentProgram.Grade.Add(newGrade);
                entity.Grades.Add(newGrade);
            }

            // existingSession.Assignments.Add(assignment); // Add this assignment to the class session

            db.AssignmentsDb.Add(entity);
            await db.SaveChangesAsync();

            return entity;
        }, context);
            
        

    }

    // UPDATE ASSIGNMENT 
    public async Task<Assignments> UpdateAssignment(Assignments updated, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetAssignmentById(updated.Id, db);

            if (existing == null)
                throw new Exception("No existing assignment found to update.");

            existing.Name = updated.Name;
            existing.TotalPoints = updated.TotalPoints;
            existing.Instructions = updated.Instructions;

            // existing.SessionId = updated.SessionId;
            // existing.Grades = updated.Grades;

            await db.SaveChangesAsync();

            return existing;

        }, context);

    }
    // DELETE ASSIGNMENT | Parameters (assignmentId , database context)
    public async Task DeleteAssignment(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(db => db.AssignmentsDb
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync()
        , context);
    }

    // GRADE Student copy of Assignment | Save to Student account, save class sesison ID in grade, save to assignment grades
    public async Task SaveGrades(List<Grade> grades, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            foreach (Grade grade in grades)
            {
                if (grade.ScoredPoints < 0) throw new ValidationException("Scores cannot be negative.");
            }

            var ids = grades
                .Select(x => x.Id)
                .ToList();

            var existingGrades = await db.GradeDb
                .Where(g => ids.Contains(g.Id))
                .ToDictionaryAsync(g => g.Id);

            foreach (var grade in grades)
            {
                if (!existingGrades.TryGetValue(grade.Id, out var existing))
                    throw new Exception($"Could not find grade {grade.Id}.");

                existing.ScoredPoints = grade.ScoredPoints;
                existing.Submitted = true;
            }

            await db.SaveChangesAsync();
        }, context);
            

        
    }

    // This function is called when an instructor submits grades for that class session. Saves and calculates if student passed or failed
    public async Task SubmitFinalGradeBySession(int sessionId, ApplicationDbContext? context = null!)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var session = await _sessionService.GetClassSessionById(sessionId, db);
            if (session == null) throw new Exception("No session found with this Id.");

            foreach (UserProgramModel program in session.StudentProgramModels)
            {
                decimal score = program.Grade.Where(x => x.SessionId == session.Id).Sum(x => x.ScoredPoints);
                decimal possiblePoints = program.Grade.Where(x => x.SessionId == sessionId).Sum(y => y.Assignment.TotalPoints);
                decimal gradePercentage = score / possiblePoints;
                if (gradePercentage >= (decimal)0.70)
                {
                    var completedCourse = new CompletedCourse()
                    {
                        SessionTakenId = sessionId,
                        SessionTaken = session,
                        CourseId = session.CourseId,
                        Course = session.Course,
                        StudentProgram = program,
                        StudentProgramId = program.Id,
                        Grade = gradePercentage,
                        DateCompleted = DateOnly.FromDateTime(DateTime.Now),
                        GPA = (decimal)4.0,
                    };
                    program.CompletedCourses.Add(completedCourse);
                    program.CurrentSessions.Remove(session);
                    session.Graduates.Add(completedCourse);
                }
                else if (gradePercentage < (decimal)0.70)
                {
                    var failedCourse = new FailedCourse()
                    {
                        CourseId = session.CourseId,
                        Course = session.Course,
                        StudentProgramId = program.Id,
                        StudentProgram = program,
                        SessionTakenId = session.Id,
                        SessionTaken = session,
                        Grade = gradePercentage,
                        DateCompleted = DateOnly.FromDateTime(DateTime.Now),
                        GPA = (decimal)1.0,
                    };

                    program.FailedSessions.Add(failedCourse);
                    program.CurrentSessions.Remove(session);
                    session.FailedCourses.Add(failedCourse);
                }
                else throw new Exception("Student grade could NOT be read. Grade could not be submitted");
            }
            await db.SaveChangesAsync();
        }, context);
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