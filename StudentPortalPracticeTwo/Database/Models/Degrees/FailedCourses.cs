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
    public decimal Grade { get; set; }
    public decimal GPA { get; set; }
    public DateOnly DateCompleted { get; set; }
}