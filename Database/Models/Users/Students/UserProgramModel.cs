using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Database.Models.Users.Students;

public class UserProgramModel
{
    [Key]
    public int Id { get; set; }

    // Student who owns this program
    public int UserId { get; set; }
    public Student? User { get; set; }

    // Degree this program is a part of
    public int DegreeId { get; set; }
    public Degree? MyDegree { get; set; } // Track Student's selected Program

    // Student Class Assignments (in the assignments table, it is tied to each classSession)
    public List<Grade> Grade { get; set; } = new(); // These are all student assignments in one storage. To find by class, use .Where(x=> x.SessionId)

    public List<ClassSession> CurrentSessions { get; set; } = new(); // Track Students Concurrent Class Sessions

    public List<Course> CompletedCourses { get; set; } = new(); // Track Completed Courses

    public List<ClassSession> RegisteredSessions { get; set; } = new(); // Tracks Student's upcoming registered classSessions for next term

    public double PercentageComplete =>// Methods to figure out current progress on degree
        MyDegree == null || MyDegree.Courses.Count() == 0
            ? 0
            : CompletedCourses.Sum(c => c.Credits) / MyDegree.Courses.Sum(c => c.Credits) * 100;
}