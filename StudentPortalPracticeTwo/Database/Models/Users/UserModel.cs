using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.Users;

public class UserModel
{
    [Key]
    public int Id { get; set; }

    // IDENTITY USER | For tokens, auth, and authorization
    public required string IdentityUserId { get; set; }
    public ApplicationUser? IdentityUser { get; set; }

    // Student settings and configurations
    public bool IsDisabled { get; set; } = false;

    // Basic Account Details
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required string Email { get; set; }

    // Other Account Details
    public required UserContactModel ContactDetails { get; set; }
    public required List<UserEmergencyContactModel> EmergencyContact { get; set; }
    
}