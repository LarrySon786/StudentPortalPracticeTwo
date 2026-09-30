using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.Users;

public class UserModel
{
    [Key]
    public int Id { get; set; }

    // IDENTITY USER | For tokens, auth, and authorization
    [Required(ErrorMessage = "Identity user ID is required")]
    [StringLength(450, ErrorMessage = "Identity user ID cannot exceed 450 characters")]
    public required string IdentityUserId { get; set; }
    public ApplicationUser? IdentityUser { get; set; }

    // Student settings and configurations
    public bool IsDisabled { get; set; } = false;

    // Basic Account Details
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
    public required string FirstName { get; set; }

    [StringLength(100, ErrorMessage = "Middle name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Middle name can only contain letters")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    public required DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public required string Email { get; set; }

    // Other Account Details
    public required UserContactModel ContactDetails { get; set; }
    public required List<UserEmergencyContactModel> EmergencyContact { get; set; }
    
}