using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users.Admin;
using StudentPortalPracticeTwo.Database.Models.Users;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Interfaces;

namespace StudentPortalPracticeTwo.Components.Services.Users;

public class AdminService
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly UserManager<ApplicationUser> _userManager;


    public AdminService(CreateDisposeContextHelper createDispose, UserManager<ApplicationUser> userManager)
    {
        _createDispose = createDispose;
        _userManager = userManager;
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
            else await _userManager.AddToRoleAsync(applicationUser, "Student"); // Assigns role for authorization

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