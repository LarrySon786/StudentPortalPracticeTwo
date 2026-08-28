using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class RoleSeeder {
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly RoleManager<IdentityRole> _roleManager;


    public RoleSeeder(IDbContextFactory<ApplicationDbContext> context, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
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
            // SEED DATA | Create roles for authorization
            if (!await _roleManager.RoleExistsAsync("Admin")) await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("Faculty")) await _roleManager.CreateAsync(new IdentityRole("Faculty"));
            if (!await _roleManager.RoleExistsAsync("Student")) await _roleManager.CreateAsync(new IdentityRole("Student"));
        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }

}