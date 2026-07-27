using System.ComponentModel.DataAnnotations;
namespace StudentPortalPracticeTwo.Database.Models.Enums;

public enum Gender
{
    [Display(Name = "Male")]
    Male,
    [Display(Name = "Female")]
    Female,
    [Display(Name = "Other")]
    Other
}