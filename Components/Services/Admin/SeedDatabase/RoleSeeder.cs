using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class RoleSeeder
{
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly RoleManager<IdentityRole> _roleManager;


    public RoleSeeder(CreateDisposeContextHelper createDispose, RoleManager<IdentityRole> roleManager)
    {
        _createDispose = createDispose;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // SEED DATA | Create roles for authorization
            if (!await _roleManager.RoleExistsAsync("Admin")) await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("Faculty")) await _roleManager.CreateAsync(new IdentityRole("Faculty"));
            if (!await _roleManager.RoleExistsAsync("Student")) await _roleManager.CreateAsync(new IdentityRole("Student"));
        }, context);
    }

}