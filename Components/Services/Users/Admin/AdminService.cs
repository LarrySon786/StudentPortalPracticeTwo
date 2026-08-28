using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users.Admin;
using StudentPortalPracticeTwo.Database.Models.Users;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Interfaces;

namespace StudentPortalPracticeTwo.Components.Services.Users;

public class AdminService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public AdminService(IDbContextFactory<ApplicationDbContext> context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET ALL Admins
    public async Task<List<AdminModel>> GetAllAdmins(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }

        try
        {
            return await context.AdminDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }

    }

    // GET Admin BY EMAIL
    public async Task<AdminModel?> GetAdminByEmail(string email, ApplicationDbContext? context = null)
    {
        bool disposeContext = false;

        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            disposeContext = true;
        }

        try
        {
            return await context.AdminDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (disposeContext == true) await context.DisposeAsync();
        }
    }

    // GET Admin BY ID
    public async Task<AdminModel?> GetAdminById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.AdminDb
                .Include(x => x.ContactDetails)
                .Include(x => x.IdentityUser)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }


    // CREATE NEW Admin | by manual creation
    public async Task CreateAdminManually(AdminModel user, ApplicationDbContext? context = null)
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
            context.AdminDb.Add(user);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // UPDATE ADMIN
    public async Task UpdateUser(AdminModel updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            AdminModel? existing = await GetAdminById(updated.Id, context);
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


    // DELETE ADMIN | Primarily for testing purposes
    public async Task DeleteAdmin(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            await context.AdminDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

}