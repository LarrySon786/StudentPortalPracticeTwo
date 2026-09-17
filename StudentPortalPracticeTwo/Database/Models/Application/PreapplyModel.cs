using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;


public class PreapplyModel
{
    [Required(ErrorMessage = "An email is required")]
    [EmailAddress]
    public string Email { get; set; } = null!;

}