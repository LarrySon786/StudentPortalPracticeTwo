using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Course
{
    // EF Core Links
    [Key]
    public int Id { get; set; }

    public List<Degree> Degrees { get; set; } = [];

    // Class Attributes
    [Required(ErrorMessage = "Course code is required")]
    [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters")]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course name is required")]
    [StringLength(200, ErrorMessage = "Course name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "Course credits must be between 0 and 100")]
    public int Credits { get; set; }

    public List<ClassSession> Sessions { get; set; } = [];


}