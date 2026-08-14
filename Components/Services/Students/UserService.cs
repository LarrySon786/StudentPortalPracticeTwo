
using StudentPortalPracticeTwo.Database.Models.Application;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Students;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using Superpower.Model;

namespace StudentPortalPracticeTwo.Components.Services.Students;

public class UserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailService emailService,
        IWebHostEnvironment environment, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
        _environment = environment;
        _configuration = configuration;
    }

    // GET ALL STUDENTS
    public async Task<List<UserModel>> GetAllUsers()
    {
        return await _context.UserDb
            .Include(x => x.ContactDetails)
            .Include(x => x.OriginalFinalApplication)
            .Include(x => x.IdentityUser)
            .ToListAsync();
    }

    // GET STUDENT BY EMAIL
    public async Task<UserModel?> GetUserByEmail(string email)
    {
        return await _context.UserDb
            .Include(x => x.ContactDetails)
            .Include(x => x.OriginalFinalApplication)
            .Include(x => x.IdentityUser)
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    // GET STUDENT BY ID
    public async Task<UserModel?> GetUserById(int id)
    {
        return await _context.UserDb
            .Include(x => x.ContactDetails)
            .Include(x => x.OriginalFinalApplication)
            .Include(x => x.IdentityUser)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    // CREATE NEW STUDENT | by finalApplication approval
    public async Task<UserModel> CreateUserByApplication(ApplicationModel finalApplication)
    {
        // Check for existing user FIRST
        var existingUser = await _userManager.FindByEmailAsync(finalApplication.Email);
        if (existingUser != null) throw new Exception($"An account already exists for {finalApplication.Email}");

        // Create User Identity for Authorization
        var applicationUser = new ApplicationUser()
        {
            Email = finalApplication.Email,
            UserName = finalApplication.Email,
            EmailConfirmed = true
        };

        // Creates the identity User
        var result = await _userManager.CreateAsync(applicationUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new Exception($"Could not create identity user. {errors}");
        }
        else await _userManager.AddToRoleAsync(applicationUser, "Student"); // Assigns role for authorization

        // Create user in Database
        List<UserEmergencyContactModel> emergencyContacts = new();
        foreach (EmergencyContactModel contact in finalApplication.EmergencyContact)
        {
            var newContact = new UserEmergencyContactModel()
            {
                ContactName = contact.ContactName,
                Phone = contact.Phone,
                Relationship = contact.Relationship,
            };
            emergencyContacts.Add(newContact);
        }

        UserModel entity = new()
        {
            FirstName = finalApplication.StudentInfo.FirstName,
            LastName = finalApplication.StudentInfo.LastName,
            DateOfBirth = finalApplication.StudentInfo.DateOfBirth,
            Email = finalApplication.Email,
            ContactDetails = new()
            {
                Phone = finalApplication.StudentContact.Phone
            },
            EmergencyContact = emergencyContacts,
            MyProgram = new()
            {
                
            },
            // Link final application to this user account
            OriginalFinalApplication = finalApplication,
            FinalApplicationId = finalApplication.Id,
            // Link identity user to this user account
            IdentityUser = applicationUser,
            IdentityUserId = applicationUser.Id
        };

        _context.UserDb.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // CREATE NEW STUDENT | by manual creation
    public async Task CreateUserManually(UserModel user)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);
        if (existingUser != null) throw new Exception($"An account already exists for {user.Email}");

        // Create User Identity for Authorization
        var applicationUser = new ApplicationUser()
        {
            Email = user.Email,
            UserName = user.Email,
            EmailConfirmed = true
        };

        // Creates the identity User
        var result = await _userManager.CreateAsync(applicationUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new Exception($"Could not create identity user. {errors}");
        }
        else await _userManager.AddToRoleAsync(applicationUser, "Student"); // Assigns role for authorization

        // Create user in Database
        _context.UserDb.Add(user);
        await _context.SaveChangesAsync();
    }

    // UPDATE STUDENT
    public async Task UpdateUser(UserModel updated)
    {
        UserModel? existing = await GetUserById(updated.Id);
        if (existing == null) throw new Exception("Could not find an existing user to update");

        existing.FirstName = updated.FirstName;
        existing.LastName = updated.LastName;
        existing.DateOfBirth = updated.DateOfBirth;
        existing.Email = updated.Email;
        existing.ContactDetails.Phone = updated.ContactDetails.Phone;

        await _context.SaveChangesAsync();
    }

    // Password Reset Email
    public async Task EmailPasswordReset(string email)
    {
        var existing = await GetUserByEmail(email);
        if (existing == null) throw new Exception("No user found with this email. Plese try again");

        // Create Email
        var htmlTemplatePath = Path.Combine(_environment.ContentRootPath, "Components", "Ui", "EmailTemplates", "ResetPassword.html"); // Approved Email Template
        var baseUrl = _configuration["AppSettings:BaseUrl"]; //Gets the base URL of the website
        var token = await _userManager.GeneratePasswordResetTokenAsync(existing.IdentityUser!); // Create token for link
        token = Uri.EscapeDataString(token); // Encodes the token
        var resetLink = $"{baseUrl}/register?userId={existing.Id}&token={token}";
        var html = await File.ReadAllTextAsync(htmlTemplatePath); //Template for approved letters

        html = html.Replace("{{reset_link}}", resetLink);
        html = html.Replace("{{first_name}}", existing.FirstName);

        // Send Email
        await _emailService.SendEmailAsync(existing.Email, existing.FirstName, "Password Reset", html);
    }

    // Reset user password (or set password for first time)
    public async Task SetPassword(string password, string token, UserModel user)
    {
        // Confirm user exists
        if (user.IdentityUser == null) throw new Exception("Cannot reset password. User not found.");
        var result = await _userManager.ResetPasswordAsync(user.IdentityUser, token, password); // reset password

        // Handle errors if result was NOT successful
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new Exception($"User was found in system, but password failed to be set/reset. {errors}");
        }
    }

    // Disable / Re-enable Student Account
    public async Task DisableUserToggle(int id)
    {
        UserModel? existing = await GetUserById(id);
        if (existing == null) throw new Exception("Could not find a user with that Id");

        existing.IsDisabled = !existing.IsDisabled;

        await _context.SaveChangesAsync();
    }

    // DELETE STUDENT | Primarily for testing purposes
    public async Task DeleteUser(int id)
    {
        await _context.UserDb
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }


    // ADMINS
    public async Task CreateAdmin(UserModel admin, string? password)
    {
        var response = await GetUserByEmail(admin.Email);
        if (response != null) throw new Exception("Cannot create admin. This email already exists with a student or admin account.");

        // Set Identity User Values
        var identityUser = new ApplicationUser()
        {
            Email = admin.Email,
            UserName = admin.Email,
            EmailConfirmed = true
        };

        // Generate email to set password
        if (password == null) password = "1234"; // CHANGE LATER

        // Create Identitty User
        var result = await _userManager.CreateAsync(identityUser, password);
        if (!result.Succeeded) throw new Exception("Could not create admin account. Failed to create identity User");
        else await _userManager.AddToRoleAsync(identityUser, "Admin"); // Set Admin Roles

        admin.IdentityUserId = identityUser.Id;
        admin.IdentityUser = identityUser;

        _context.Add(admin);
        await _context.SaveChangesAsync();
    }



}