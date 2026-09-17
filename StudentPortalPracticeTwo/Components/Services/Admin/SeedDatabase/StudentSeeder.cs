using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class StudentSeeder
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly StudentService _studentService;

    public StudentSeeder(CreateDisposeContextHelper createDispose, StudentService studentService
)
    {
        _createDispose = createDispose;
        _studentService = studentService;

    }
    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // SEED DATA | Create Students
            var json = File.ReadAllText("Database/JSON/Users/Students/Students.json");
            var studentDefinition = JsonSerializer.Deserialize<List<JsonStudent>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            foreach (JsonStudent student in studentDefinition!)
            {
                await _studentService.CreateDTOStudent(student, db);
            }

        }, context);
    }
}