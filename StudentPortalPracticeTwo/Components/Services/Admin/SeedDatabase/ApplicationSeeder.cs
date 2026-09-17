using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class ApplicationSeeder
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly TermService _termService;
    private readonly DegreeService _degreeService;


    public ApplicationSeeder(CreateDisposeContextHelper createDispose,
        TermService termService, DegreeService degreeService)
    {
        _createDispose = createDispose;

        _termService = termService;
        _degreeService = degreeService;
    }


    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // SEED DATA | Create Submited Application
            var file = File.ReadAllText("Database/JSON/Applications/FinalApplication.json");
            var applicationDefinition = JsonSerializer.Deserialize<List<JsonFinalApplication>>(
                file, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                });



            foreach (var student in applicationDefinition!)
            {
                // Get existing Degree and Term from database
                var identifiedDegree = await _degreeService.GetDegreeById(student.StudentProgram.DegreeId, db);
                var identifiedTerm = await _termService.GetTermById(student.StudentProgram.TermId, db);


                ApplicationModel newApplication = new()
                {
                    Email = student.Email,
                    ApprovedStatus = student.ApprovedStatus,

                    // Student Information
                    StudentInfo = new StudentInfoModel()
                    {
                        FirstName = student.StudentInfo.FirstName,
                        MiddleName = student.StudentInfo.MiddleName,
                        LastName = student.StudentInfo.LastName,
                        DateOfBirth = student.StudentInfo.DateOfBirth,
                        Race = student.StudentInfo.Race,
                        Gender = student.StudentInfo.Gender,
                        CitizenshipCountry = student.StudentInfo.CitizenshipCountry,
                        StreetOneAddress = student.StudentInfo.StreetOneAddress,
                        StreetTwoAddress = student.StudentInfo.StreetTwoAddress,
                        City = student.StudentInfo.City,
                        StateOrProvince = student.StudentInfo.StateOrProvince,
                        Zipcode = student.StudentInfo.Zipcode
                    },

                    // Student Contact
                    StudentContact = new StudentContactModel()
                    {
                        Phone = student.StudentContact.Phone,
                        AltPhone = student.StudentContact.AltPhone
                    },

                    // Emergency Contacts
                    EmergencyContact = student.EmergencyContacts
                        .Select(contact => new EmergencyContactModel()
                        {
                            ContactName = contact.ContactName,
                            Relationship = contact.Relationship,
                            Phone = contact.Phone
                        })
                        .ToList(),

                    // Student Program
                    StudentProgram = new StudentProgram()
                    {
                        SelectedProgram = identifiedDegree!,
                        StartTerm = identifiedTerm
                    },

                    // Academic History
                    AcademicHistory = new AcademicHistoryModel()
                    {
                        HighschoolTranscriptFileName = student.AcademicHistory.HighschoolTranscriptFileName,

                        HighschoolTranscript = Convert.FromBase64String(student.AcademicHistory.HighschoolTranscript),

                        CollegeTranscriptFileName = student.AcademicHistory.CollegeTranscriptFileName,

                        CollegeTranscript = student.AcademicHistory.CollegeTranscript == null
                                ? null
                                : Convert.FromBase64String(student.AcademicHistory.CollegeTranscript)
                    },

                    // Essays
                    Essays = new StudentEssayModel()
                    {
                        ResponseOne = student.Essays.ResponseOne,
                        ResponseTwo = student.Essays.ResponseTwo,
                        ResponseThree = student.Essays.ResponseThree
                    }
                };
                db.ApplicationDb.Add(newApplication);
            }
        }, context);
    }
}