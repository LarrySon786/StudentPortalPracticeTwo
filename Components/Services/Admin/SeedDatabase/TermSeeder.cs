using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;

public class TermSeeder
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    private readonly TermService _termService;


    public TermSeeder(IDbContextFactory<ApplicationDbContext> context, TermService termService)
    {
        _context = context;
        _termService = termService;
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
            // SEED DATA | Create Default Terms
            var termOne = new Term() // Term One
            {
                Season = TermSeason.Fall,
                Year = 2026,
                AvailableToRegisterClasses = true,
            };

            var termTwo = new Term() // Term Two
            {
                Season = TermSeason.Spring,
                Year = 2027,
                AvailableToRegisterClasses = true,
            };
            await _termService.CreateTerm(termOne, context); // Create Terms
            await _termService.CreateTerm(termTwo, context);

        }
        finally
        {
            if (dispose == true) await context.DisposeAsync();
        }
    }
}