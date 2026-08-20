using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Users.Instructors;

public class FacultyService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly UserService _userService;

    public FacultyService(IDbContextFactory<ApplicationDbContext> context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }

    // GET ALL FACULTY
    public async Task<List<Faculty>> GetAllFaculty(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await FacultyQuery(context)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // GET FACULTY BY EMAIL
    public async Task<Faculty?> GetByEmail(string email, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await FacultyQuery(context)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // GET FACULTY BY ID
    public async Task<Faculty?> GetById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await FacultyQuery(context)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // CREATE NEW FACULTY
    public async Task<Faculty> CreateFaculty(Faculty faculty, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            // Check if faculty user already exists / if email is in use
            var existing = await GetByEmail(faculty.Email, context);
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

            context.FacultyUsers.Add(entity);
            await context.SaveChangesAsync(); // Save Faculty Member
            return entity;

        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // UPDATE FACULTY
    public async Task<Faculty> UpdateFaculty(Faculty updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            // Check if faculty user already exists / if email is in use
            var existing = await GetById(updated.Id, context);
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


            await context.SaveChangesAsync(); // Save Faculty Member Updates
            return existing;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
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
            .Include(x => x.IdentityUser)
            .Include(x => x.ContactDetails)
            .Include(x => x.EmergencyContact);
    }

}
