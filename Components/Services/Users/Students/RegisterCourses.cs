// This Service Class is to allow students to register for their own courses

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Users.Students;

public class RegisterCourses
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly StudentService _studentService;
    private readonly ClassSessionService _classSessionService;


    public RegisterCourses(IDbContextFactory<ApplicationDbContext> context, StudentService studentService, ClassSessionService classSessionService)
    {
        _context = context;
        _studentService = studentService;
        _classSessionService = classSessionService;
    }

    public async Task<List<ClassSession>> RegisterNewCourses(Student user, List<ClassSession> sessions, ApplicationDbContext? context = null)
    {
        // arrange the user data and EF core tracking
        bool dispose = false;
        Student? existing;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
            existing = await _studentService.GetStudentById(user.Id, context);
        }
        else
        {
            existing = user;
        }
        
        // Register courses to user account
        try
        {
            if (existing == null) throw new Exception("No user found to register courses for.");

            var sessionIds = sessions.Select(x => x.Id).ToList();

            List<ClassSession> existingSessions = await context.ClassSessionDb
                .Where(x => sessionIds.Contains(x.Id))
                .Include(x => x.RegisteredStudentProgramModels)
                .ToListAsync();

            foreach (ClassSession session in existingSessions)
            {
                existing.MyProgram.RegisteredSessions.Add(session);
            }

            await context.SaveChangesAsync();
            return existing.MyProgram.RegisteredSessions;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }
}

