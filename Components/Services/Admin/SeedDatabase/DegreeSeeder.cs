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

public class DegreeSeeder
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly DegreeService _degreeService;


    public DegreeSeeder(IDbContextFactory<ApplicationDbContext> context, DegreeService degreeService)
    {
        _context = context;
        _degreeService = degreeService;
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
            // SEED DATA || Create Default Degree
            var jsonDegree = File.ReadAllText("Database/JSON/Degrees/SoftwareEngineering.json"); // Get JSON of degree
            var degreeDefinition = JsonSerializer.Deserialize<JsonDegree>(
                jsonDegree, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            Degree degree = new() // Create Degree
            {
                Name = "Software Engineering",
                Description = "Software Engineering is the finnest dicipline.",
                Courses = [],
            };

            foreach (string code in degreeDefinition!.Courses) // Add all courses
            {
                var course = context.CourseDb.Single(c => c.Code == code);
                degree.Courses.Add(course);
            }
            
            await _degreeService.CreateDegree(degree, context); // Create + Save


        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }

}