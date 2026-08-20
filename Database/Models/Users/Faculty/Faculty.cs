using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Database.Models.Users.Faculty;

public class Faculty : UserModel
{
    public List<ClassSession> ClassSessions { get; set; } = new();
}