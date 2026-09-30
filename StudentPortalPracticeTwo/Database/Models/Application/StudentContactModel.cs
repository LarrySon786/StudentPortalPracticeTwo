

using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;


public class StudentContactModel
{
    public int Id { get; set; }

    public ApplicationModel Application { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Must be a valid phone number.")]
    [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters")]
    public required string Phone { get; set; }

    [Phone(ErrorMessage = "Must be a valid alternative phone number.")]
    [StringLength(30, ErrorMessage = "Alternative phone number cannot exceed 30 characters")]
    public string? AltPhone { get; set; }

}