using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Degree
{
    // EF Core Links
    public int Id { get; set; }

    // Degree Attributes
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    public List<Course> Courses { get; set; } = [];

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;


}





