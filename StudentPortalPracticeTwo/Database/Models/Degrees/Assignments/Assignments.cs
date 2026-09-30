// This model are assignments given by faculty to students and then grades

using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

// One assignment is owned by one student. It will need created for each student in the class (so a loop)

public class Assignments
{
    [Key]
    public int Id { get; set; }

    // Links to Session Assignment which then links to the Grade (then to student through Grade)
    public List<Grade> Grades { get; set; } = new();

    // Links to Class Session
    public ClassSession? Session { get; set; }
    public int SessionId { get; set; }

    // Properties
    [Required(ErrorMessage = "Assignment name is required")]
    [StringLength(200, ErrorMessage = "Assignment name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Assignment instructions cannot exceed 2000 characters")]
    public string Instructions { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Assignment total points must be at least 1")]
    public int TotalPoints { get; set; }


}