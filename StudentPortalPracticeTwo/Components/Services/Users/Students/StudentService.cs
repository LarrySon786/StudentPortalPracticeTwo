
using StudentPortalPracticeTwo.Database.Models.Application;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Users.Students;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Components.Services.Users.Students;

public class StudentService
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly UserManager<ApplicationUser> _userManager;


    public StudentService(CreateDisposeContextHelper createDispose, UserManager<ApplicationUser> userManager)
    {
        _createDispose = createDispose;
        _userManager = userManager;

    }

    // GET ALL STUDENTS
    public async Task<List<Student>> GetAllStudents(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => StudentQuery(db)
                .ToListAsync(), context);

    }

    // GET STUDENT BY EMAIL
    public async Task<Student?> GetStudentByEmail(string email, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => StudentQuery(db)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync(), context);
    }

    // GET STUDENT BY ID
    public async Task<Student?> GetStudentById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => StudentQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(), context);
    }

    // CREATE NEW STUDENT | by finalApplication approval
    public async Task<CreateStudentResultHelper> CreateStudentByApplication(ApplicationModel finalApplication, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
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

            db.StudentDb.Add(entity);
            await db.SaveChangesAsync();
            return new CreateStudentResultHelper
            {
                User = entity,
                ApplicationUser = applicationUser
            };
        }, context);
    }

    // CREATE NEW STUDENT | by manual creation
    public async Task CreateStudentManually(Student user, ApplicationDbContext? context = null)
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
            db.StudentDb.Add(user);
            await db.SaveChangesAsync();
        }, context);
    }

    // Create Student from DTO
    public async Task CreateDTOStudent(JsonStudent student, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
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
            var degree = await db.DegreeDb
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
            }
            ;

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

                ClassSession? classSession = await db.ClassSessionDb
                    .Where(x => x.StartTime == sessionDto.StartTime && x.Course.Code == sessionDto.CourseCode
                        && x.Instructor!.Email == sessionDto.InstructorEmail && x.Term!.Year == year && x.Term.Season == season)
                    .FirstOrDefaultAsync();

                if (classSession == null) throw new Exception("Class session did not match either start time, course code, instructor email, or existing term.");

                currentSessions.Add(classSession);
            }

            // Registered Sessions
            List<ClassSession> registeredSessions = new();
            foreach (JsonClassSessionReference sessionDto in student.RegisteredClassSessions)
            {
                string seasonString = new string(
                    sessionDto.Term.Where(char.IsLetter).ToArray()
                );

                string yearString = new string(
                    sessionDto.Term.Where(char.IsDigit).ToArray()
                );

                TermSeason season = Enum.Parse<TermSeason>(seasonString);
                int year = int.Parse(yearString);

                ClassSession? classSession = await db.ClassSessionDb
                    .Where(x => x.StartTime == sessionDto.StartTime && x.Course.Code == sessionDto.CourseCode
                        && x.Instructor!.Email == sessionDto.InstructorEmail && x.Term!.Year == year && x.Term.Season == season)
                    .FirstOrDefaultAsync();

                if (classSession == null) throw new Exception("Class session did not match either start time, course code, instructor email, or existing term.");

                registeredSessions.Add(classSession);
            }

            // Compeleted Courses
            List<CompletedCourse> completedCourses = new();
            foreach (JsonClassSessionReference sessionDto in student.RegisteredClassSessions)
            {
                string seasonString = new string(
                    sessionDto.Term.Where(char.IsLetter).ToArray()
                );

                string yearString = new string(
                    sessionDto.Term.Where(char.IsDigit).ToArray()
                );

                TermSeason season = Enum.Parse<TermSeason>(seasonString);
                int year = int.Parse(yearString);

                ClassSession? classSession = await db.ClassSessionDb
                    .Where(x => x.StartTime == sessionDto.StartTime && x.Course.Code == sessionDto.CourseCode
                        && x.Instructor!.Email == sessionDto.InstructorEmail && x.Term!.Year == year && x.Term.Season == season)
                    .FirstOrDefaultAsync();

                if (classSession == null) throw new Exception("Class session did not match either start time, course code, instructor email, or existing term.");

                CompletedCourse course = new()
                {
                    SessionTaken = classSession,
                    SessionTakenId = classSession.Id,
                    Course = classSession.Course,
                    CourseId = classSession.CourseId,
                };

                completedCourses.Add(course);
            }


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
                    RegisteredSessions = registeredSessions ?? [],
                    CompletedCourses = completedCourses ?? [],
                },
                IdentityUserId = applicationUser.Id,
            };

            if (completedCourses != null)
            {
                foreach (CompletedCourse course in completedCourses)
                {
                    course.StudentProgram = entity.MyProgram;
                    course.StudentProgramId = entity.MyProgram.Id;
                }
            }

            // Create user in Database
            db.StudentDb.Add(entity);
            await db.SaveChangesAsync();
        }, context);
    }

    // UPDATE STUDENT
    public async Task<Student> UpdateUser(Student updated, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            Student? existing = await GetStudentById(updated.Id, db);
            if (existing == null) throw new Exception("Could not find an existing user to update");

            existing.FirstName = updated.FirstName;
            existing.LastName = updated.LastName;
            existing.DateOfBirth = updated.DateOfBirth;
            existing.Email = updated.Email;
            existing.ContactDetails.Phone = updated.ContactDetails.Phone;

            await db.SaveChangesAsync();
            return existing;
        }, context);
    }

    // DELETE STUDENT | Primarily for testing purposes
    public async Task DeleteStudent(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(db => db.StudentDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(), context);
    }


    private IQueryable<Student> StudentQuery(ApplicationDbContext context)
    {
        return context.StudentDb.Include(x => x.ContactDetails)
                .Include(x => x.OriginalFinalApplication)
                .Include(x => x.IdentityUser)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.MyDegree)
                        .ThenInclude(x => x!.Courses)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.RegisteredSessions)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CompletedCourses)
                        .ThenInclude(x => x.Course)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.CurrentSessions)
                        .ThenInclude(x => x.Course)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.FailedSessions)
                        .ThenInclude(x => x.Course)
                .Include(x => x.MyProgram)
                    .ThenInclude(x => x.Grade)
                        .ThenInclude(x => x.Assignment)
                .Include(x => x.EmergencyContact);
    }


}