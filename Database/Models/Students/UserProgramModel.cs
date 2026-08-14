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
    public Degree? MyDegree { get; set; }

    // Track Completed Courses

    // Track Current Courses

    // Upcoming Registered Courses

    // Methods to figure out current progress || In a service
}