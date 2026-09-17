using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
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
    private readonly CreateDisposeContextHelper _createDispose;

    private readonly TermService _termService;


    public TermSeeder(CreateDisposeContextHelper createDispose, TermService termService)
    {
        _createDispose = createDispose;
        _termService = termService;
    }
    public async Task SeedAsync(ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
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
            await _termService.CreateTerm(termOne, db); // Create Terms
            await _termService.CreateTerm(termTwo, db);

        }, context);
    }
}