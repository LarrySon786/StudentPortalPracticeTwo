// This Service Class is to allow students to register for their own courses

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Users.Students;

public class RegisterCourses
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly StudentService _studentService;
    private readonly ClassSessionService _classSessionService;


    public RegisterCourses(CreateDisposeContextHelper createDispose, StudentService studentService, ClassSessionService classSessionService)
    {
        _createDispose = createDispose;
        _studentService = studentService;
        _classSessionService = classSessionService;
    }

    public async Task<List<ClassSession>> RegisterNewCourses(Student user, List<ClassSession> sessions, ApplicationDbContext? context = null)
    {
        // arrange the user data and EF core tracking
        return await _createDispose.ExecuteAsync(async db =>
        {
            Student? existing = context == null
                ? await _studentService.GetStudentById(user.Id, db)
                : user;

            if (existing == null) throw new Exception("No user found to register courses for.");

            var sessionIds = sessions.Select(x => x.Id).ToList();

            List<ClassSession> existingSessions = await db.ClassSessionDb
                .Where(x => sessionIds.Contains(x.Id))
                .Include(x => x.RegisteredStudentProgramModels)
                .ToListAsync();

            foreach (ClassSession session in existingSessions)
            {
                existing.MyProgram.RegisteredSessions.Add(session);
            }

            await db.SaveChangesAsync();
            return existing.MyProgram.RegisteredSessions;
        }, context);
    }
}

