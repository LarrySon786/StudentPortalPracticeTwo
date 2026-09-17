using System.ComponentModel.DataAnnotations;
namespace StudentPortalPracticeTwo.Database.Models.Enums;

public enum SupportStatus
{
    [Display(Name = "Resolved")]
    Resolved,
    [Display(Name = "Awaiting Student Response")]
    Awaiting,
    [Display(Name = "Pending")]
    Pending,
    [Display(Name = "Submitted")]
    Submitted, // Used for a submitted request that has NOT been reviewed by admins yet.
    [Display(Name = "Any")] 
    Any
}