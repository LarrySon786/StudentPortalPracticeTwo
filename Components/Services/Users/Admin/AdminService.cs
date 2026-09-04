using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users.Admin;
using StudentPortalPracticeTwo.Database.Models.Users;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace StudentPortalPracticeTwo.Components.Services.Users;

public class AdminService
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;


    public AdminService(CreateDisposeContextHelper createDispose, UserManager<ApplicationUser> userManager,
        IEmailService emailService, IWebHostEnvironment environment, IConfiguration configuration)
    {
        _createDispose = createDispose;
        _userManager = userManager;
        _emailService = emailService;
        _environment = environment;
        _configuration = configuration;
    }

    public async Task InviteAdmin(PendingAdmin pending)
    {
        if (string.IsNullOrWhiteSpace(pending.Email))
            throw new ArgumentException("An email is required", nameof(pending));

        if (await _userManager.FindByEmailAsync(pending.Email) != null ||
            await GetAdminByEmail(pending.Email) != null)
            throw new InvalidOperationException($"An account already exists for {pending.Email}");

        string token = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
        pending.HashedInviteToken = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        await _createDispose.ExecuteAsync(async db =>
        {
            db.PendingAdminDb.Add(pending);
            await db.SaveChangesAsync();
        });

        string baseUrl = _configuration["AppSettings:BaseUrl"] ?? string.Empty;
        string registrationLink = $"{baseUrl}/admin/accept-invite?userId={pending.Id}&token={token}";
        string templatePath = Path.Combine(_environment.ContentRootPath, "Components", "Ui", "EmailTemplates", "NewAdmin.html");
        string html = await File.ReadAllTextAsync(templatePath);
        string fullName = $"{pending.FirstName} {pending.LastName}";
        html = html.Replace("{{Admin_Registration_Link}}", registrationLink)
            .Replace("{{Admin_First_And_Last_Name}}", fullName);

        await _emailService.SendEmailAsync(pending.Email, fullName,
            "ACTION REQUIRED | New Administrator Registration | CSU Administration", html);
    }

    public async Task<PendingAdmin?> GetPendingAdmin(int id)
    {
        return await _createDispose.ExecuteAsync(db => db.PendingAdminDb
            .FirstOrDefaultAsync(admin => admin.Id == id));
    }

    public async Task AcceptAdminInvite(PendingAdmin pending, string token, string password,
        DateOnly dateOfBirth, string phone)
    {
        string tokenHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));
        if (!string.Equals(tokenHash, pending.HashedInviteToken, StringComparison.Ordinal))
            throw new InvalidOperationException("This administrator invitation is invalid.");

        if (string.IsNullOrWhiteSpace(pending.Email) || string.IsNullOrWhiteSpace(pending.FirstName) ||
            string.IsNullOrWhiteSpace(pending.LastName))
            throw new InvalidOperationException("This administrator invitation is incomplete.");

        var identity = new ApplicationUser
        {
            UserName = pending.Email,
            Email = pending.Email,
            EmailConfirmed = true
        };
        var result = await _userManager.CreateAsync(identity, password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(error => error.Description)));

        await _userManager.AddToRoleAsync(identity, "Admin");
        await _createDispose.ExecuteAsync(async db =>
        {
            db.AdminDb.Add(new AdminModel
            {
                FirstName = pending.FirstName,
                LastName = pending.LastName,
                Email = pending.Email,
                DateOfBirth = dateOfBirth,
                ContactDetails = new() { Phone = phone },
                EmergencyContact = new(),
                IdentityUserId = identity.Id
            });
            db.PendingAdminDb.Remove(pending);
            await db.SaveChangesAsync();
        });
    }

    // GET ALL Admins
    public async Task<List<AdminModel>> GetAllAdmins(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.AdminDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .ToListAsync(), context);
    }

    // GET Admin BY EMAIL
    public async Task<AdminModel?> GetAdminByEmail(string email, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.AdminDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync(), context);
    }

    // GET Admin BY ID
    public async Task<AdminModel?> GetAdminById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.AdminDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(), context);
    }


    // CREATE NEW Admin | by manual creation
    public async Task CreateAdminManually(AdminModel user, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
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
            else await _userManager.AddToRoleAsync(applicationUser, "Admin"); // Assigns role for authorization

            // Create user in Database
            db.AdminDb.Add(user);
            await db.SaveChangesAsync();
        }, context);
    }

    // UPDATE ADMIN
    public async Task UpdateUser(AdminModel updated, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            AdminModel? existing = await GetAdminById(updated.Id, db);
            if (existing == null) throw new Exception("Could not find an existing user to update");

            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.DateOfBirth = updated.DateOfBirth;
            existing.Email = updated.Email;
            existing.ContactDetails.Phone = updated.ContactDetails.Phone;

            await db.SaveChangesAsync();
        }, context);
    }


    // DELETE ADMIN | Primarily for testing purposes
    public async Task DeleteAdmin(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(db => db.AdminDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(), context);
    }

}