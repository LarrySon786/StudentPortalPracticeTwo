using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class StudentEssayModel
{
    [Key]
    public int Id { get; set; }

    public ApplicationModel? Application { get; set; }
    public int ApplicationId { get; set; }


    [Required(ErrorMessage = "Essay responses are required.")]
    [MaxLength(3000, ErrorMessage = "Essay response one cannot exceed 3000 characters")]
    [MinLength(300, ErrorMessage = "Essay response one must be at least 300 characters")]
    public string ResponseOne { get; set; } = null!;


    [Required(ErrorMessage = "Essay responses are required.")]
    [MaxLength(3000, ErrorMessage = "Essay response two cannot exceed 3000 characters")]
    [MinLength(300, ErrorMessage = "Essay response two must be at least 300 characters")]
    public string ResponseTwo { get; set; } = null!;
    
    
    [Required(ErrorMessage = "Essay responses are required.")]
    [MaxLength(3000, ErrorMessage = "Essay response three cannot exceed 3000 characters")]
    [MinLength(300, ErrorMessage = "Essay response three must be at least 300 characters")]
    public string ResponseThree { get; set; } = null!;
}