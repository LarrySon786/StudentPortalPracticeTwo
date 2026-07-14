using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Database.Models.Students;

public class UserModel
{
    [Key]
    public int Id { get; set; }
    public int FinalApplicationId { get; set; }
    public ApplicationModel? OriginalFinalApplication { get; set; } = null;

    public bool isDisabled { get; set; } = false;

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required UserContactModel ContactDetails { get; set; }
    
}