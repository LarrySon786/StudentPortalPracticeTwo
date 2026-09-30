using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class FailedCourse
{
    [Key]
    public int Id { get; set; }


    // Reference to the course this is
    public Course Course { get; set; } = null!;
    public int CourseId { get; set; }


    // Reference to session taken
    public ClassSession? SessionTaken { get; set; }
    public int SessionTakenId { get; set; }


    // Reference to student
    public UserProgramModel StudentProgram { get; set; } = null!;
    public int StudentProgramId { get; set; }


    // Properties
    [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
    public decimal Grade { get; set; }

    [Range(0, 4, ErrorMessage = "GPA must be between 0 and 4")]
    public decimal GPA { get; set; }
    public DateOnly DateCompleted { get; set; }
}