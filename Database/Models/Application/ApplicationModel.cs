

using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;


public class ApplicationModel
{
    [Key]
    public int Id { get; set; }
    public Status ApprovedStatus { get; set; } = Status.Pending;


    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Student info is required")]
    public required StudentInfoModel StudentInfo { get; set; }

    [Required(ErrorMessage = "Student contact is required")]
    public required StudentContactModel StudentContact { get; set; }

    [Required(ErrorMessage = "Emergency contacts are required")]
    public required List<EmergencyContactModel> EmergencyContact { get; set; }
}
public enum Status
{
    Approved,
    Pending,
    Denied,
    All
}