using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Enums;

public enum Race
{
    [Display(Name = "White / Caucasion")]
    CaucasionWhite,
    [Display(Name = "African")]
    African,
    [Display(Name = "Hispanic")]
    Hispanic,
    [Display(Name = "Native American")]
    NativeAmerican,
    [Display(Name = "Asian")]
    Asian,
    [Display(Name = "Other")]
    Other,
}