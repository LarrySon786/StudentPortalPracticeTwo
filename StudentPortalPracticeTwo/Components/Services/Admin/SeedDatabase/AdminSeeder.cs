using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class AdminSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly CreateDisposeContextHelper _createDispose;

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


    public AdminSeeder(ApplicationDbContext db, CreateDisposeContextHelper createDispose, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, UserService userService, StudentService studentService,
        FacultyService facultyService, AdminService adminService, RoleManager<IdentityRole> roleManager,
        TermService termService, DegreeService degreeService, CourseService courseService,
        ClassSessionService classSessionService)
    {
        _db = db;
        _createDispose = createDispose;

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
        await _createDispose.ExecuteAsync(async db =>
        {
            // SEED DATA


        }, context);
    }

}