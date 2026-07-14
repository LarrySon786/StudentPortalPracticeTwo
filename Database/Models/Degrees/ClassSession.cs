using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class ClassSession
{
    // EF Core Links
    [Key]
    public int Id { get; set; }

    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    // Session attributes
    [Required]
    [StringLength(100)]
    public string Instructor { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    // Assign dates and times to the class (and school block)
    [Required]
    [StringLength(50)]
    public string Term { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly StartTime { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly EndTime { get; set; }


    // Count how many students are registered for this class
    [Range(0, int.MaxValue)]
    public int CurrentCount { get; set; }

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

}