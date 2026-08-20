// This Service Class is to allow students to register for their own courses

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Users.Students;

public class RegisterCourses
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly StudentService _studentService;


    public RegisterCourses(IDbContextFactory<ApplicationDbContext> context, StudentService studentService)
    {
        _context = context;
        _studentService = studentService;
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

            foreach (ClassSession session in sessions)
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

