
using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Users;

public class UserEmergencyContactModel
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserModel? User { get; set; }

    [Required(ErrorMessage = "Contact name is required")]
    public required string ContactName { get; set; }

    [Required(ErrorMessage = "Contact relationship is required")]
    public required string Relationship { get; set; }

    [Required(ErrorMessage = "Contact phone number is required")]
    [Phone(ErrorMessage = "Must be a valid phone number")]
    public required string Phone { get; set; }



}