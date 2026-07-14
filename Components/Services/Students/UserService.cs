
using StudentPortalPracticeTwo.Database.Models.Application;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Students;

namespace StudentPortalPracticeTwo.Components.Services.Students;

public class UserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL STUDENTS
    public async Task<List<UserModel>> GetAllUsers()
    {
        return await _context.UserDb
            .Include(x => x.ContactDetails)
            .Include(x => x.OriginalFinalApplication)
            .ToListAsync();
    }

    // GET STUDENT BY EMAIL
    public async Task<UserModel?> GetUserByEmail(string email)
    {
        return await _context.UserDb
            .Include(x => x.ContactDetails)
            .Include(x => x.OriginalFinalApplication)
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();
    }

    // GET STUDENT BY ID
    public async Task<UserModel?> GetUserById(int id)
    {
        return await _context.UserDb
            .Include(x => x.ContactDetails)
            .Include(x => x.OriginalFinalApplication)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    // CREATE NEW STUDENT | by finalApplication approval
    public async Task CreateUserByApplication(ApplicationModel finalApplication)
    {
        UserModel entity = new()
        {
            FirstName = finalApplication.StudentInfo.FirstName,
            LastName = finalApplication.StudentInfo.LastName,
            Email = finalApplication.Email,
            ContactDetails = new()
            {
                Phone = finalApplication.StudentContact.Phone
            },
            OriginalFinalApplication = finalApplication,
            FinalApplicationId = finalApplication.Id
        };

        _context.UserDb.Add(entity);
        await _context.SaveChangesAsync();
    }

    // CREATE NEW STUDENT | by manual creation
    public async Task CreateUserManually(UserModel user)
    {
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
        existing.Email = updated.Email;
        existing.ContactDetails.Phone = updated.ContactDetails.Phone;

        await _context.SaveChangesAsync();
    }

    // Disable / Re-enable Student Account
    public async Task DisableUserToggle(int id)
    {
        UserModel? existing = await GetUserById(id);
        if (existing == null) throw new Exception("Could not find a user with that Id");

        existing.isDisabled = !existing.isDisabled;

        await _context.SaveChangesAsync();
    }

    // DELETE STUDENT | Primarily for testing purposes
    public async Task DeleteUser(int id)
    {
        await _context.UserDb
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }
}