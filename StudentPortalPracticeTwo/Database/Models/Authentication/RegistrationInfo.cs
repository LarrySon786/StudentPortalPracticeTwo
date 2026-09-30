
using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Authentication;

public class RegistrationInfo
{
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}