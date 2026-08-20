using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public class CreateStudentResultHelper
{
    public Student? User { get; set; } = null;
    public ApplicationUser? ApplicationUser { get; set; } = null;
}