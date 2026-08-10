using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class StudentEssayModel
{
    [Key]
    public int Id { get; set; }

    public ApplicationModel? Application { get; set; }
    public int ApplicationId { get; set; }


    [Required(ErrorMessage = "Essay responses are required.")]
    [MaxLength(700, ErrorMessage = "Essay response one must be less that 700 characters")]
    [MinLength(300, ErrorMessage = "Essay response one must be over 300 characters")]
    public string ResponseOne { get; set; } = null!;


    [Required(ErrorMessage = "Essay responses are required.")]
    [MaxLength(700, ErrorMessage = "Essay response two must be less that 700 characters")]
    [MinLength(300, ErrorMessage = "Essay response two must be over 300 characters")]
    public string ResponseTwo { get; set; } = null!;
    
    
    [Required(ErrorMessage = "Essay responses are required.")]
    [MaxLength(700, ErrorMessage = "Essay response three must be less that 700 characters")]
    [MinLength(300, ErrorMessage = "Essay response three must be over 300 characters")]
    public string ResponseThree { get; set; } = null!;
}