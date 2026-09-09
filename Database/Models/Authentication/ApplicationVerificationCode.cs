
using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Database.Models.Authentication;

public class ApplicationVerificationCode
{
    [Key]
    public int Id { get; set; }

    // Reference the draft application
    public int DraftApplicationId { get; set; }
    public DraftApplicationModel? DraftApplication { get; set; }


    // Properties
    public string HashedCode { get; set; } = string.Empty;

    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    public DateTime ExpirationTime { get; set; } = DateTime.UtcNow.AddMinutes(15);

    public bool Used { get; set; } = false;

    public int FailedAttempts { get; set; }

}
