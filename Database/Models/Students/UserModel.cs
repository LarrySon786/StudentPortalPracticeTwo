using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Database.Models.Students;

public class UserModel
{
    [Key]
    public int Id { get; set; }

    // Student's Admissions Application
    public int? FinalApplicationId { get; set; }
    public ApplicationModel? OriginalFinalApplication { get; set; } = null;

    // IDENTITY USER | For tokens, auth, and authorization
    public required string IdentityUserId { get; set; }
    public ApplicationUser? IdentityUser { get; set; }

    // Student settings and configurations
    public bool IsDisabled { get; set; } = false;

    // Student Account Details
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Email { get; set; }
    public required UserContactModel ContactDetails { get; set; }
    
}