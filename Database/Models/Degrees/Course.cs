using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Course
{
    // EF Core Links
    [Key]
    public int Id { get; set; }

    public List<Degree> Degrees { get; set; } = [];

    // Class Attributes
    [Required]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, 100)]
    public int Credits { get; set; }

    public List<ClassSession> Sessions { get; set; } = [];


}