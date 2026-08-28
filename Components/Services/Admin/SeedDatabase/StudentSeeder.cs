using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class StudentSeeder
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly StudentService _studentService;

    public StudentSeeder(IDbContextFactory<ApplicationDbContext> context, StudentService studentService
)
    {
        _context = context;
        _studentService = studentService;

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
            // SEED DATA | Create Students
            var json = File.ReadAllText("Database/JSON/Users/Students/Students.json");
            var studentDefinition = JsonSerializer.Deserialize<List<JsonStudent>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            foreach (JsonStudent student in studentDefinition!)
            {
                await _studentService.CreateDTOStudent(student, context);
            }

        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }
}