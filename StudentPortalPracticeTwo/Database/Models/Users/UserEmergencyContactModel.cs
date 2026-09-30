
using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Users;

public class UserEmergencyContactModel
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserModel? User { get; set; }

    [Required(ErrorMessage = "Contact name is required")]
    [StringLength(200, ErrorMessage = "Contact name cannot exceed 200 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
    public required string ContactName { get; set; }

    [Required(ErrorMessage = "Contact relationship is required")]
    [StringLength(100, ErrorMessage = "Contact relationship cannot exceed 100 characters")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
    public required string Relationship { get; set; }

    [Required(ErrorMessage = "Contact phone number is required")]
    [Phone(ErrorMessage = "Must be a valid phone number")]
    [StringLength(30, ErrorMessage = "Phone number cannot exceed 30 characters")]
    public required string Phone { get; set; }



}