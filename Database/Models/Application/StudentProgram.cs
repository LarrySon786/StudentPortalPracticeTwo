
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class StudentProgram
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public ApplicationModel Application { get; set; } = null!;

    public required Degree SelectedProgram { get; set; }

    public required Term StartTerm { get; set; }
}