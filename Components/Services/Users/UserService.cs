
using StudentPortalPracticeTwo.Database.Models.Application;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using StudentPortalPracticeTwo.Components.Services.Extensions;

namespace StudentPortalPracticeTwo.Components.Services.Users;

public class UserService
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;

    public UserService(CreateDisposeContextHelper createDispose, UserManager<ApplicationUser> userManager, IEmailService emailService,
        IWebHostEnvironment environment, IConfiguration configuration)
    {
        _createDispose = createDispose;
        _userManager = userManager;
        _emailService = emailService;
        _environment = environment;
        _configuration = configuration;
    }

    // GET ALL Users
    public async Task<List<UserModel>> GetAllUsers(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .ToListAsync(), context);
    }

    // GET User BY EMAIL
    public async Task<UserModel?> GetUserByEmail(string email, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync(), context);
    }

    // GET User BY ID
    public async Task<UserModel?> GetUserById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(), context);
    }

    // UPDATE STUDENT
    public async Task<UserModel> UpdateUser(UserModel updated, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            UserModel? existing = await GetUserById(updated.Id, db);
            if (existing == null) throw new Exception("Could not find an existing user to update");

            existing.FirstName = updated.FirstName;
            existing.MiddleName = updated.MiddleName;
            existing.LastName = updated.LastName;
            existing.DateOfBirth = updated.DateOfBirth;
            existing.Email = updated.Email;
            existing.ContactDetails.Phone = updated.ContactDetails.Phone;

            await db.SaveChangesAsync();

            return existing;
        }, context);
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
    public async Task<bool> DisableUserToggle(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            UserModel? existing = await GetUserById(id, db);
            if (existing == null) throw new Exception("Could not find a user with that Id");

            existing.IsDisabled = !existing.IsDisabled;

            await db.SaveChangesAsync();
            return existing.IsDisabled;
        }, context);
    }

    // DELETE USER | Primarily for testing purposes
    public async Task DeleteUser(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(db => db.UserDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(), context);
    }

}