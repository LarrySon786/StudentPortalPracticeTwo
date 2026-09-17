

using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class DraftStudentInfoModel
{
    public int Id { get; set; }

    public DraftApplicationModel Application { get; set; } = null!;

    [Display(Name = "First Name")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string? FirstName { get; set; }

    [Display(Name = "Middle Name")]
    [StringLength(100, ErrorMessage = "Middle name cannot exceed 100 characters")]
    public string? MiddleName { get; set; }

    [Display(Name = "Last Name")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string? LastName { get; set; }

    [Display(Name = "Date of Birth")]
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Race")]
    public Race? Race { get; set; }

    [Display(Name = "Gender")]
    public Gender? Gender { get; set; }

    [Display(Name = "Citizenship Country")]
    public Country? CitizenshipCountry { get; set; }

    [Display(Name = "Street Address 1")]
    [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
    public string? StreetOneAddress { get; set; }

    [Display(Name = "Street Address 2")]
    [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
    public string? StreetTwoAddress { get; set; }

    [Display(Name = "City")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string? City { get; set; }

    [Display(Name = "State or Province")]
    [StringLength(100, ErrorMessage = "State or province cannot exceed 100 characters")]
    public string? StateOrProvince { get; set; }

    [Display(Name = "Zip Code")]
    [Range(10000, 99999, ErrorMessage = "Zip code must be between 10000 and 99999")]
    public int? Zipcode { get; set; }
}