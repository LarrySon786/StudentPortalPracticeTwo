
using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Authentication;

public class Login
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}