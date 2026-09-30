
using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class StudentProgram
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public ApplicationModel Application { get; set; } = null!;

    [Required(ErrorMessage = "A student program is required")]
    public required Degree SelectedProgram { get; set; }

    [Required(ErrorMessage = "A start term is required")]
    public required Term StartTerm { get; set; }
}