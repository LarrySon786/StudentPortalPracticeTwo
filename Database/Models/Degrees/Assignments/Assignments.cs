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
    public string Name { get; set; } = string.Empty;
    public string Instructions { get; set; } = string.Empty;
    public int TotalPoints { get; set; }


}