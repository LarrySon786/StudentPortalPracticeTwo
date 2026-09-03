using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class CourseSeeder
{
    private readonly CreateDisposeContextHelper _createDispose;


    private readonly CourseService _courseService;



    public CourseSeeder(CreateDisposeContextHelper createDispose, CourseService courseService)
    {
        _createDispose = createDispose;
        _courseService = courseService;
    }

    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // SEED DATA || Create Courses
            var jsonCourses = File.ReadAllText("Database/JSON/Courses/SoftwareEngineering.json"); // Course JSON
            var courseDefinition = JsonSerializer.Deserialize<List<JsonCourse>>(jsonCourses, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (JsonCourse courseItem in courseDefinition!) // Iterate through all courses to create
            {
                var course = new Course()
                {
                    Name = courseItem.Name,
                    Code = courseItem.Code,
                    Credits = courseItem.Credits,
                };
                await _courseService.CreateCourse(course, db); // Create + Save
            }

        }, context);
    }
}