using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Users.Students;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class ClassSession
{
    // EF Core Links
    [Key]
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a course.")]
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Please select a term.")]
    public int TermId { get; set; } // Term Relationship for database
    public Term? Term { get; set; }

    public List<UserProgramModel> StudentProgramModels { get; set; } = new();
    public List<UserProgramModel> RegisteredStudentProgramModels { get; set; } = new();

    // Completed Course Link
    public List<CompletedCourse> Graduates { get; set; } = new();

    // Failed Course Link
    public List<FailedCourse> FailedCourses { get; set; } = new();

    // public Class Assignments
    public List<Assignments> Assignments { get; set; } = new(); // This list of assignments needs to be looped and created for EACH student

    [Range(1, int.MaxValue, ErrorMessage = "Please select an instructor.")]
    public int InstructorId { get; set; }
    public Faculty? Instructor { get; set; }

    // Session attributes
    [Required(ErrorMessage = "Class location is required")]
    [StringLength(200, ErrorMessage = "Class location cannot exceed 200 characters")]
    public string Location { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Class description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;

    // Assign dates and times to the class (and school block)
    

    [DataType(DataType.Date)]
    public DateOnly StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly EndDate { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly StartTime { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly EndTime { get; set; }



    [Range(0, int.MaxValue, ErrorMessage = "Current class count cannot be negative")]
    public int CurrentCount { get; set; } // TO DO: Make this property calculated by counting Users[]

    [Range(1, int.MaxValue, ErrorMessage = "Class capacity must be at least 1")]
    public int Capacity { get; set; }

    public bool ArchivedAndClosed { get; set; } = false;
    public bool ClassStarted { get; set; } = false;

}