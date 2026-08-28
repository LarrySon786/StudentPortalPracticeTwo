
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class ResetDatabase
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly SignInManager<ApplicationUser> _signInManager;

    // SEEDERS
    private readonly RoleSeeder _roleSeeder;
    private readonly TermSeeder   _termSeeder ;
    private readonly CourseSeeder   _courseSeeder ;
    private readonly DegreeSeeder   _degreeSeeder;
    private readonly AdminSeeder   _adminSeeder;
    private readonly FacultySeeder   _facultySeeder;
    private readonly ClassSessionSeeder   _classSessionSeeder;
    private readonly StudentSeeder   _studentSeeder;
    private readonly ApplicationSeeder _applicationSeeder;
    private readonly DevSeeder _devSeeder;


    public ResetDatabase(IDbContextFactory<ApplicationDbContext> context, SignInManager<ApplicationUser> signInManager,
        // New seeders
        RoleSeeder roleSeeder, TermSeeder termSeeder, CourseSeeder courseSeeder, DegreeSeeder degreeSeeder,
        AdminSeeder adminSeeder, FacultySeeder facultySeeder, ClassSessionSeeder classSessionSeeder,
        StudentSeeder studentSeeder, ApplicationSeeder applicationSeeder, DevSeeder devSeeder)
        {
            _context = context;
            _signInManager = signInManager;

            // New seeders
            _roleSeeder = roleSeeder;
            _termSeeder = termSeeder;
            _courseSeeder = courseSeeder;
            _degreeSeeder = degreeSeeder;
            _adminSeeder = adminSeeder;
            _facultySeeder = facultySeeder;
            _classSessionSeeder = classSessionSeeder;
            _studentSeeder = studentSeeder;
            _applicationSeeder = applicationSeeder;
            _devSeeder = devSeeder;
        }


    public async Task ResetAsync()
    {
        var context = await _context.CreateDbContextAsync();

        // Sign out current user || Needs to be done over HTTP, but not essential to reset at this time.
        // await _signInManager.SignOutAsync();

        // Delete existing database
        await context.Database.EnsureDeletedAsync();

        // Recreate database using current migrations
        await context.Database.MigrateAsync();

        // Seed database in dependency order
        await _roleSeeder.SeedAsync(context);

        await _termSeeder.SeedAsync(context);

        await _courseSeeder.SeedAsync(context);

        await _degreeSeeder.SeedAsync(context); // Happens after courses are created

        await _devSeeder.SeedAsync(context); // Must happen after degrees are created

        await _adminSeeder.SeedAsync(context);

        await _facultySeeder.SeedAsync(context);

        await _classSessionSeeder.SeedAsync(context); // Happens after COURSES and FACULTY are created

        await _studentSeeder.SeedAsync(context); // Happens after Class Sessions are seeded

        await _applicationSeeder.SeedAsync(context); // Happens last

        

        await context.SaveChangesAsync();
    }
}


