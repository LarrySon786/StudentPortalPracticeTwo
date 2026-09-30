using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Users;

public class UserContactModel
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserModel? User { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Enter a valid phone number")]
    [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters")]
    public required string Phone { get; set; }
}