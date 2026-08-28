using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class ClassSessionSeeder
{

    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly ClassSessionService _classSessionService;


    public ClassSessionSeeder(IDbContextFactory<ApplicationDbContext> context,
        ClassSessionService classSessionService)
    {
        _context = context;
        _classSessionService = classSessionService;
    }

    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            // SEED DATA | Create Course Sessions
            var jsonSessions = File.ReadAllText("Database/JSON/ClassSessions.Json"); // Read Json Sample Data
            var classSessionDefinition = JsonSerializer.Deserialize<List<JsonClassSessions>>( // Convert to DTO
                jsonSessions, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            foreach (var session in classSessionDefinition!) // Iterate and create each JSON DTO object as session
            {
                var course = context.CourseDb.Single(s => s.Code == session.CourseCode); // Get course, term, and instructor
                var term = context.TermDb.Single(s => s.Season.ToString() + s.Year.ToString() == $"{session.Term}");
                var instructor = context.FacultyDb.Single(s => s.Email == session.InstructorEmail);

                var createdSession = new ClassSession()
                {
                    Course = course,
                    Instructor = instructor,
                    Location = session.Location,
                    Capacity = session.Capacity,
                    CurrentCount = session.CurrentCount,
                    StartDate = session.StartDate,
                    StartTime = session.StartTime,
                    EndDate = session.EndDate,
                    EndTime = session.EndTime,
                    Description = session.Description,
                    Term = term,
                };

                await _classSessionService.CreateClassSession(createdSession, context); // Create and save
            }
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }

}