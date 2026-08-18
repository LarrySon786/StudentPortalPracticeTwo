
using StudentPortalPracticeTwo.Database.Models.Application;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Students;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using Superpower.Model;
using StudentPortalPracticeTwo.Components.Services.Extensions;

namespace StudentPortalPracticeTwo.Components.Services.Students;

public class UserService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public UserService(IDbContextFactory<ApplicationDbContext> context, UserManager<ApplicationUser> userManager, IEmailService emailService,
        IWebHostEnvironment environment, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
        _environment = environment;
        _configuration = configuration;
    }

    // GET ALL STUDENTS
    public async Task<List<UserModel>> GetAllUsers(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }

        try
        {
            return await context.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
        
    }

    // GET STUDENT BY EMAIL
    public async Task<UserModel?> GetUserByEmail(string email, ApplicationDbContext? context = null)
    {
        bool disposeContext = false;

        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            disposeContext = true;
        }

        try
        {
            return  await context.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (disposeContext == true) await context.DisposeAsync();
        }
    }

    // GET STUDENT BY ID
    public async Task<UserModel?> GetUserById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
        }

    // CREATE NEW STUDENT | by finalApplication approval
    public async Task<CreateUserResultHelper> CreateUserByApplication(ApplicationModel finalApplication, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }

        try
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

            // Create User ROLE
            var roleResult = await _userManager.AddToRoleAsync(applicationUser, "Student");
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
                throw new Exception($"Could not assign Student role. {errors}");
            }

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
                    DegreeId = finalApplication.StudentProgram.SelectedProgram.Id,
                },
                // Link final application to this user account
                FinalApplicationId = finalApplication.Id,
                OriginalFinalApplication = finalApplication,
                // Link identity user to this user account
                IdentityUserId = applicationUser.Id,
            };

            context.UserDb.Add(entity);
            await context.SaveChangesAsync();
            return new CreateUserResultHelper
            {
                User = entity,
                ApplicationUser = applicationUser
            };
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // CREATE NEW STUDENT | by manual creation
    public async Task CreateUserManually(UserModel user, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
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
            context.UserDb.Add(user);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // UPDATE STUDENT
    public async Task UpdateUser(UserModel updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            UserModel? existing = await GetUserById(updated.Id, context);
            if (existing == null) throw new Exception("Could not find an existing user to update");

            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.DateOfBirth = updated.DateOfBirth;
            existing.Email = updated.Email;
            existing.ContactDetails.Phone = updated.ContactDetails.Phone;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
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
        Console.WriteLine($"RESETTING PASSWORD");
        Console.WriteLine($"Identity ID: {user.IdentityUser!.Id}");
        Console.WriteLine($"Security Stamp: {user.IdentityUser.SecurityStamp}");
        Console.WriteLine($"Email: {user.IdentityUser.Email}");
        Console.WriteLine($"Token Length: {token.Length}");
        
        // Find Identity User
        var identityUser = await _userManager.FindByIdAsync(user.IdentityUserId);

        // Confirm user exists
        if (identityUser == null) throw new Exception("Cannot reset password. User not found.");
        var result = await _userManager.ResetPasswordAsync(identityUser, token, password); // reset password

        // Handle errors if result was NOT successful
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new Exception($"User was found in system, but password failed to be set/reset. {errors}");
        }
    }

    // Disable / Re-enable Student Account
    public async Task DisableUserToggle(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            UserModel? existing = await GetUserById(id, context);
            if (existing == null) throw new Exception("Could not find a user with that Id");

            existing.IsDisabled = !existing.IsDisabled;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // DELETE STUDENT | Primarily for testing purposes
    public async Task DeleteUser(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            await context.UserDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }


    // ADMINS
    public async Task CreateAdmin(UserModel admin, string? password, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var response = await GetUserByEmail(admin.Email, context);
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

            context.Add(admin);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

}