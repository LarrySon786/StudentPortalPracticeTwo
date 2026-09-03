using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace StudentPortalPracticeTwo.Components.Services.Users.Instructors;

public class FacultyService
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly UserService _userService;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _environment; // Allows tracing back to root of project
    private readonly IConfiguration _configuration; // Gets base URL for the project 

    public FacultyService(CreateDisposeContextHelper createDispose, UserService userService,
        IEmailService emailService, IWebHostEnvironment enviroment, IConfiguration configuration)
    {
        _createDispose = createDispose;
        _userService = userService;
        _emailService = emailService;
        _environment = enviroment;
        _configuration = configuration;
    }

    // GET ALL FACULTY
    public async Task<List<Faculty>> GetAllFaculty(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => FacultyQuery(db)
                .ToListAsync(), context);
    }

    // GET FACULTY BY EMAIL
    public async Task<Faculty?> GetByEmail(string email, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => FacultyQuery(db)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync(), context);
    }

    // GET FACULTY BY ID
    public async Task<Faculty?> GetById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => FacultyQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(), context);
    }

    // INVITE NEW FACULTY || This method sends an email invite to a new faculty member
    public async Task InviteFaculty(PendingFaculty pending)
    {
        // Server side Validation

        // Form Email and Token
        string firstLastName = $"{pending.FirstName} {pending.LastName}";
        string subject = "ACTION REQUIRED | New Faculty Registration | CSU Administration";
        var htmlTemplatePath = Path.Combine(_environment.ContentRootPath, "Components", "Ui", "EmailTemplates", "NewFaculty.html"); // Approved Email Template
        var baseUrl = _configuration["AppSettings:BaseUrl"]; //Gets the base URL of the website
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", ""); ;
        pending.HashedInviteToken = Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(token))
        );
        // Store hash in db
        await _createDispose.ExecuteAsync(async db =>
        {
            db.PendingFacultyDb.Add(pending);
            await db.SaveChangesAsync();
        });
        var registrationLink = $"{baseUrl}/faculty/accept-invite?userId={pending.Id}&token={token}";
        var html = await File.ReadAllTextAsync(htmlTemplatePath); //Template 

        html = html.Replace("{{Faculty_Registration_Link}}", registrationLink);
        html = html.Replace("{{Faculty_First_And_Last_Name}}", firstLastName);

        // Send Invite via Email
        await _emailService.SendEmailAsync(pending.Email!, firstLastName, subject, html);
    }

    // CREATE NEW FACULTY
    public async Task<Faculty> CreateFaculty(Faculty faculty, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            // Check if faculty user already exists / if email is in use
            var existing = await GetByEmail(faculty.Email, db);
            if (existing != null) throw new Exception("A faculty or student user already exist with this email");

            List<UserEmergencyContactModel> emergencyContacts = new();

            foreach (var contact in faculty.EmergencyContact)
            {
                emergencyContacts.Add(contact); // Create Emergency Contacts
            }


            Faculty entity = new() // Create faculty member
            {
                ClassSessions = faculty.ClassSessions,
                Email = faculty.Email,
                FirstName = faculty.FirstName,
                LastName = faculty.LastName,
                DateOfBirth = faculty.DateOfBirth,
                EmergencyContact = emergencyContacts,
                ContactDetails = new()
                {
                    Phone = faculty.ContactDetails.Phone
                },
                IdentityUserId = faculty.IdentityUserId,
            };

            db.FacultyUsers.Add(entity);
            await db.SaveChangesAsync(); // Save Faculty Member
            return entity;
        }, context);
    }

    // UPDATE FACULTY
    public async Task<Faculty> UpdateFaculty(Faculty updated, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            // Check if faculty user already exists / if email is in use
            var existing = await GetById(updated.Id, db);
            if (existing == null) throw new Exception("No faculty member could be found to update");

            List<UserEmergencyContactModel> emergencyContacts = new();
            foreach (var contact in updated.EmergencyContact)
            {
                emergencyContacts.Add(contact); // Create Emergency Contacts
            }

            // Create faculty member

            existing.ClassSessions = updated.ClassSessions;
            existing.Email = updated.Email;
            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.DateOfBirth = updated.DateOfBirth;
            existing.EmergencyContact = emergencyContacts;
            existing.ContactDetails = updated.ContactDetails;
            existing.IdentityUserId = updated.IdentityUserId;


            await db.SaveChangesAsync(); // Save Faculty Member Updates
            return existing;
        }, context);
    }

    // DISABLE FACULTY
    public async Task DisableFaculty(int id, ApplicationDbContext? context = null)
    {
        await _userService.DisableUserToggle(id, context); // Uses disable feature from userService
    }


    private IQueryable<Faculty> FacultyQuery(ApplicationDbContext context)
    {
        return context.FacultyUsers
            .Include(x => x.ClassSessions)
                .ThenInclude(x => x.Term)
            .Include(x => x.ClassSessions)
                .ThenInclude(x => x.Course)
            .Include(x => x.IdentityUser)
            .Include(x => x.ContactDetails)
            .Include(x => x.EmergencyContact);
    }

}
