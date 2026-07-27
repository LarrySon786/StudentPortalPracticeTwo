using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class DraftEmergencyContactModel
{
    [Key]
    public int Id { get; set; }

    public int DraftApplicationId { get; set; }
    public DraftApplicationModel? Application { get; set; }

    public string ContactName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Must be a valid phone number")]
    public string Phone { get; set; } = string.Empty;
}