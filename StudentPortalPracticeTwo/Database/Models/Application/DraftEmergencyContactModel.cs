using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class DraftEmergencyContactModel
{
    [Key]
    public int Id { get; set; }

    public int DraftApplicationId { get; set; }
    public DraftApplicationModel? Application { get; set; }

    [StringLength(200, ErrorMessage = "Contact name cannot exceed 100 characters")]
    public string ContactName { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Relationship cannot exceed 100 characters")]
    public string Relationship { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Must be a valid phone number")]
    public string Phone { get; set; } = string.Empty;
}