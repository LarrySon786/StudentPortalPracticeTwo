

using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class StudentInfoModel
{
    public int Id { get; set; }

    public ApplicationModel Application { get; set; } = null!;

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public required string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Middle name is required")]
    [Display(Name = "Middle Name")]
    [StringLength(100, ErrorMessage = "Middle name cannot exceed 100 characters")]
    public required string MiddleName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public required string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Race is required")]
    [Display(Name = "Race")]
    public required Race Race { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    [Display(Name = "Gender")]
    public required Gender Gender { get; set; }

    [Required(ErrorMessage = "Citizenship country is required")]
    [Display(Name = "Citizenship Country")]
    public required Country CitizenshipCountry { get; set; }

    [Required(ErrorMessage = "Street address is required")]
    [Display(Name = "Street Address 1")]
    [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
    public required string StreetOneAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Street address is required")]
    [Display(Name = "Street Address 2")]
    [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
    public required string StreetTwoAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [Display(Name = "City")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public required string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State or province is required")]
    [Display(Name = "State or Province")]
    [StringLength(100, ErrorMessage = "State or province cannot exceed 100 characters")]
    public required string StateOrProvince { get; set; } = string.Empty;

    [Required(ErrorMessage = "Zip code is required")]
    [Display(Name = "Zip Code")]
    [Range(10000, 99999, ErrorMessage = "Zip code must be between 10000 and 99999")]
    public required int Zipcode { get; set; }
}