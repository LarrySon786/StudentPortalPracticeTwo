using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Students;

public class UserContactModel
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserModel? User { get; set; }

    public required string Phone { get; set; }
}