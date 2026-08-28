using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Admin;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class DevSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly UserService _userService;
    private readonly StudentService _studentService;
    private readonly FacultyService _facultyService;
    private readonly AdminService _adminService;

    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly TermService _termService;
    private readonly DegreeService _degreeService;
    private readonly CourseService _courseService;
    private readonly ClassSessionService _classSessionService;


    public DevSeeder(ApplicationDbContext db, IDbContextFactory<ApplicationDbContext> context, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, UserService userService, StudentService studentService,
        FacultyService facultyService, AdminService adminService, RoleManager<IdentityRole> roleManager,
        TermService termService, DegreeService degreeService, CourseService courseService,
        ClassSessionService classSessionService)
    {
        _db = db;
        _context = context;

        _signInManager = signInManager;
        _userManager = userManager;

        _userService = userService;
        _studentService = studentService;
        _facultyService = facultyService;
        _adminService = adminService;

        _roleManager = roleManager;

        _termService = termService;
        _degreeService = degreeService;
        _courseService = courseService;
        _classSessionService = classSessionService;
    }

    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            // SEED DATA || Admin User
            ApplicationUser newAdminIdentity = new()
            {
                Email = "richardsbrandon4@gmail.com",
                UserName = "richardsbrandon4@gmail.com",
            };

            var resultOne = await _userManager.CreateAsync(newAdminIdentity, "Adminiscool1234$");
            if (!resultOne.Succeeded) throw new Exception("Could not create admin dev account");
            else await _userManager.AddToRoleAsync(newAdminIdentity, "Admin"); // Set Admin Roles

            UserEmergencyContactModel emergencyContactNumber = new()
            {
                ContactName = "Angie",
                Phone = "330-333-3333",
                Relationship = "Mother",
            };
            var admin = new AdminModel()
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "richardsbrandon4@gmail.com",
                DateOfBirth = new DateOnly(2001, 6, 26),
                ContactDetails = new()
                {
                    Phone = "111-222-3333"
                },
                EmergencyContact = [emergencyContactNumber],
                IdentityUserId = newAdminIdentity.Id,
            };

            context.AdminDb.Add(admin);
            await context.SaveChangesAsync();



            // Seed Data || Faculty User
            ApplicationUser devFacultyIdentity = new()
            {
                UserName = "richardsbrandon5@gmail.com",
                Email = "richardsbrandon5@gmail.com",
            };
            var resultFacultyDev = await _userManager.CreateAsync(devFacultyIdentity, "Instructoriscool1234$");
            if (!resultFacultyDev.Succeeded) throw new Exception("Could not create developer faculty user.");
            else await _userManager.AddToRoleAsync(devFacultyIdentity, "Faculty"); // Set Faculty Roles

            

            var devFaculty = new Faculty() // Faculty Creation
            {
                FirstName = "Faculty",
                LastName = "User",
                Email = "richardsbrandon5@gmail.com",
                DateOfBirth = new DateOnly(2001, 6, 26),
                ContactDetails = new()
                {
                    Phone = "111-222-3333"
                },
                EmergencyContact = [ new UserEmergencyContactModel
                {
                    ContactName = "Angie",
                    Phone = "330-333-3333",
                    Relationship = "Mother"
                }],
                IdentityUserId = devFacultyIdentity.Id,
            };
            await _facultyService.CreateFaculty(devFaculty, context);



            // Seed Data || Registered Student
            ApplicationUser newStudentIdentity = new() // User Identity for student login
            {
                UserName = "richardsbrandon2@gmail.com",
                Email = "richardsbrandon2@gmail.com",
            };

            var result2 = await _userManager.CreateAsync(newStudentIdentity, "Studentiscool1234$");
            if (!result2.Succeeded) throw new Exception("Could not create student dev account.");
            else await _userManager.AddToRoleAsync(newStudentIdentity, "Student"); // Set Admin Roles

            UserEmergencyContactModel contact = new() // Emergency contact for student
            {
                ContactName = "James Richards",
                Relationship = "Father",
                Phone = "666-777-8888",
            };

            var degree = await context.DegreeDb // Get degree to assign to student
                .Where(x => x.Name == "Software Engineering")
                .Include(x => x.Courses)
                    .ThenInclude(x => x.Sessions)
                .FirstOrDefaultAsync();

            if (degree is null)
                throw new InvalidOperationException(
                    "Software Engineering degree must exist before seeding students.");

            var student = new Student() // Create Student
            {
                FirstName = "Student",
                MiddleName = "Real",
                LastName = "Account",
                DateOfBirth = new DateOnly(1990, 3, 25),
                Email = "richardsbrandon2@gmail.com",
                ContactDetails = new()
                {
                    Phone = "111-222-3333"
                },
                EmergencyContact = [contact],
                MyProgram = new()
                {
                    DegreeId = degree.Id,
                    CompletedCourses = [],
                    RegisteredSessions = [],
                    CurrentSessions = [],
                    Grade = [],
                },
                IdentityUserId = newStudentIdentity.Id
            };

            context.StudentDb.Add(student);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }
}