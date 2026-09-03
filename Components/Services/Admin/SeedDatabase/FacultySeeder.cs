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
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class FacultySeeder
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FacultyService _facultyService;


    public FacultySeeder(CreateDisposeContextHelper createDispose, UserManager<ApplicationUser> userManager,
        FacultyService facultyService)
    {
        _createDispose = createDispose;
        _userManager = userManager;
        _facultyService = facultyService;
    }
    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // SEED DATA | Create Instructors
            var jsonInstructors = File.ReadAllText("Database/JSON/Users/Faculty/Faculty.Json");
            var facultyDefinition = JsonSerializer.Deserialize<List<JsonFaculty>>(
                jsonInstructors, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            foreach (JsonFaculty faculty in facultyDefinition!)
            {
                // Create faculty member's user Identity
                var identity = new ApplicationUser()
                {
                    UserName = faculty.Email,
                    Email = faculty.Email,
                };

                var result = await _userManager.CreateAsync(identity); // Create sign-in identity
                if (!result.Succeeded) throw new Exception("Could not create Instructor's user Identity in database seeding.");

                result = await _userManager.AddToRoleAsync(identity, "Faculty"); // Assign role
                if (!result.Succeeded) throw new Exception("Could not set Instructor's role in database seeding.");

                List<UserEmergencyContactModel> emergencyContacts = new(); // Prepare emergency contacts
                foreach (var contact in faculty.EmergencyContact)
                {
                    var newContact = new UserEmergencyContactModel()
                    {
                        ContactName = contact.ContactName,
                        Relationship = contact.Relationship,
                        Phone = contact.Phone,
                    };
                    emergencyContacts.Add(newContact);
                }

                var entity = new Faculty() // Create each faculty member
                {
                    FirstName = faculty.FirstName,
                    MiddleName = faculty.MiddleName,
                    LastName = faculty.LastName,
                    Email = faculty.Email,
                    DateOfBirth = faculty.DateOfBirth,
                    ContactDetails = new()
                    {
                        Phone = faculty.ContactDetails.Phone
                    },
                    EmergencyContact = emergencyContacts,
                    IdentityUserId = identity.Id
                };
                await _facultyService.CreateFaculty(entity, db); // Create and Save
            }

        }, context);
    }
}