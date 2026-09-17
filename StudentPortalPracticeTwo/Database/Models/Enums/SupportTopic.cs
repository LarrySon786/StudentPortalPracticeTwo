using System.ComponentModel.DataAnnotations;
namespace StudentPortalPracticeTwo.Database.Models.Enums;

public enum SupportTopic
{
    [Display(Name = "Tech Support")]
    Technical,
    [Display(Name = "Class Registeration")]
    ClassRegisteration,
    [Display(Name = "Student Portal Issues")]
    StudentPortalIssues,
    [Display(Name = "Grades")]
    Grade,
    [Display(Name = "Graduation")]
    Graduation,
    [Display(Name = "Other")]
    Other
}