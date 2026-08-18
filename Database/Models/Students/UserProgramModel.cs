using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Database.Models.Students;

public class UserProgramModel
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserModel? User { get; set; }

    public int DegreeId { get; set; }
    public Degree? MyDegree { get; set; } // Track Student's selected Program

    public List<ClassSession> CurrentSessions { get; set; } = new(); // Track Students Concurrent Class Sessions

    public List<Course> CompletedCourses { get; set; } = new(); // Track Completed Courses

    public List<ClassSession> RegisteredSessions { get; set; } = new(); // Tracks Student's upcoming registered classSessions for next term

    public double PercentageComplete =>// Methods to figure out current progress on degree
        MyDegree == null || MyDegree.Courses.Count() == 0
            ? 0
            : CompletedCourses.Sum(c => c.Credits) / MyDegree.Courses.Sum(c => c.Credits) * 100;
}