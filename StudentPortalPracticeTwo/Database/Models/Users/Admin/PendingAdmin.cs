using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Users.Admin;

public class PendingAdmin
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A first name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "A last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "An email is required")]
    [EmailAddress(ErrorMessage = "Must be a valid email")]
    [StringLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string? Email { get; set; }

    public string? HashedInviteToken { get; set; } = string.Empty;
}