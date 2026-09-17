using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Database.Models.Users.Students;

public class Student : UserModel
{
    // Student's Admissions Application
    public int? FinalApplicationId { get; set; }
    public ApplicationModel? OriginalFinalApplication { get; set; } = null;

    public required UserProgramModel MyProgram { get; set; }
}