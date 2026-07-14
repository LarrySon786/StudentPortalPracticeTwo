using Microsoft.AspNetCore.Identity;

namespace StudentPortalPracticeTwo.Database.Models.Account;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}