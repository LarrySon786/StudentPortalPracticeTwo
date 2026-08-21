// This model is used to collect data to send an invite to a real faculty member

using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Users.Faculty;

public class PendingFaculty
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A first name is required")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "A last name is required")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "An email is required")]
    [EmailAddress(ErrorMessage = "Must be a valid email")]
    public string? Email { get; set; }

    public string? HashedInviteToken { get; set; } = string.Empty;
}