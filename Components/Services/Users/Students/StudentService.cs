
using StudentPortalPracticeTwo.Database.Models.Application;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Users.Students;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Components.Services.Users.Students;

public class StudentService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly UserManager<ApplicationUser> _userManager;


    public StudentService(IDbContextFactory<ApplicationDbContext> context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }

    // GET ALL STUDENTS
    public async Task<List<Student>> GetAllStudents(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }

        try
        {
            return await context.StudentDb
                .Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.MyDegree)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.RegisteredSessions)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CompletedCourses)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CurrentSessions)
                .Include(x => x.EmergencyContact)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }

    }

    // GET STUDENT BY EMAIL
    public async Task<Student?> GetStudentByEmail(string email, ApplicationDbContext? context = null)
    {
        bool disposeContext = false;

        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            disposeContext = true;
        }

        try
        {
            return await context.StudentDb
                .Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.MyDegree)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.RegisteredSessions)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CompletedCourses)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CurrentSessions)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (disposeContext == true) await context.DisposeAsync();
        }
    }

    // GET STUDENT BY ID
    public async Task<Student?> GetStudentById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.StudentDb
                .Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.MyDegree)
                    .ThenInclude(x => x!.Courses)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.RegisteredSessions)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CompletedCourses)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CurrentSessions)
                .Include(x => x.EmergencyContact)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // CREATE NEW STUDENT | by finalApplication approval
    public async Task<CreateStudentResultHelper> CreateStudentByApplication(ApplicationModel finalApplication, ApplicationDbContext? context = null)
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

            Student entity = new()
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

            context.StudentDb.Add(entity);
            await context.SaveChangesAsync();
            return new CreateStudentResultHelper
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
    public async Task CreateStudentManually(Student user, ApplicationDbContext? context = null)
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
            context.StudentDb.Add(user);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // Create Student from DTO
    public async Task CreateDTOStudent(JsonStudent student, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(student.Email);
            if (existingUser != null) throw new Exception($"An account already exists for {student.Email}");

            // Create User Identity for Authorization
            var applicationUser = new ApplicationUser()
            {
                Email = student.Email,
                UserName = student.Email,
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

            // Get Degree Id for user
            var degree = await context.DegreeDb
                .Where(x => x.Name == student.DegreeName)
                .FirstOrDefaultAsync();

            if (degree == null)
                throw new Exception($"Degree '{student.DegreeName}' does not exist.");

            // Create emergency contacts for user
            var contacts = new List<UserEmergencyContactModel>();
            foreach (JsonUserEmergencyContact contact in student.EmergencyContact)
            {
                UserEmergencyContactModel emergencyContact = new()
                {
                    ContactName = contact.ContactName,
                    Relationship = contact.Relationship,
                    Phone = contact.Phone,
                };
                contacts.Add(emergencyContact);
            };

            // Current Sessions 
            List<ClassSession> currentSessions = new();
            foreach (JsonClassSessionReference sessionDto in student.CurrentClassSessions)
            {
                string seasonString = new string(
                    sessionDto.Term.Where(char.IsLetter).ToArray()
                );

                string yearString = new string(
                    sessionDto.Term.Where(char.IsDigit).ToArray()
                );

                TermSeason season = Enum.Parse<TermSeason>(seasonString);
                int year = int.Parse(yearString);

                ClassSession? classSession = await context.ClassSessionDb
                    .Where(x => x.StartTime == sessionDto.StartTime && x.Course.Code == sessionDto.CourseCode
                        && x.Instructor!.Email == sessionDto.InstructorEmail && x.Term!.Year  == year && x.Term.Season == season)
                    .FirstOrDefaultAsync();

                if (classSession == null) throw new Exception("Class session did not match either start time, course code, instructor email, or existing term.");

                currentSessions.Add(classSession);
            }

            // Registered Sessions


            // Compeleted Sessions



            // Create Student Entity
            var entity = new Student()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                MiddleName = student.MiddleName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                ContactDetails = new()
                {
                    Phone = student.ContactDetails.Phone
                },
                EmergencyContact = contacts,
                MyProgram = new()
                {
                    DegreeId = degree.Id,
                    CurrentSessions = currentSessions ?? [],
                    RegisteredSessions = [],
                    CompletedCourses = [],
                },
                IdentityUserId = applicationUser.Id,
            };
            
            // Create user in Database
            context.StudentDb.Add(entity);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // UPDATE STUDENT
    public async Task<Student> UpdateUser(Student updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            Student? existing = await GetStudentById(updated.Id, context);
            if (existing == null) throw new Exception("Could not find an existing user to update");

            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.DateOfBirth = updated.DateOfBirth;
            existing.Email = updated.Email;
            existing.ContactDetails.Phone = updated.ContactDetails.Phone;

            await context.SaveChangesAsync();
            return existing;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // DELETE STUDENT | Primarily for testing purposes
    public async Task DeleteStudent(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            await context.StudentDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

}