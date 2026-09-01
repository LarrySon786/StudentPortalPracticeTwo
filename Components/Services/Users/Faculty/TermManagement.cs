using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class TermManagement
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly ClassSessionService _sessionService;

    public TermManagement(IDbContextFactory<ApplicationDbContext> context, ClassSessionService sessionService)
    {
        _context = context;
        _sessionService = sessionService;
    }

    public async Task StartTerm(int classSessionId, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existing = await _sessionService.GetClassSessionById(classSessionId, context);
            if (existing == null) throw new Exception("Could not fin an existing class session by Id to start.");

            if (existing.RegisteredStudentProgramModels.Count() == 0) throw new Exception("No students are enrolled in this class");

            foreach (UserProgramModel program in existing.RegisteredStudentProgramModels)
            {
                program.CurrentSessions.Add(existing);
                program.RegisteredSessions.Remove(existing);

                foreach (Assignments assignment in existing.Assignments)
                {
                    program.Grade.Add(new Grade
                    {
                        Assignment = assignment,
                        AssignmentId = assignment.Id,
                        StudentProgram = program,
                        StudentProgramId = program.Id,
                        Session = existing,
                        SessionId = existing.Id,
                    });
                }

            }
            existing.ClassStarted = true;
            
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

}