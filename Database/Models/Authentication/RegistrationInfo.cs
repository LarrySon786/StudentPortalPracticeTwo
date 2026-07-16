
using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Authentication;

public class RegistrationInfo
{
    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}