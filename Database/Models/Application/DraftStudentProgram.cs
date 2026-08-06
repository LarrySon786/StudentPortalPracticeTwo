using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class DraftStudentProgram
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public DraftApplicationModel? Application { get; set; }

    public int? SelectedProgramId { get; set; }
    public Degree? SelectedProgram { get; set; }

    public int? StartTermId { get; set; }
    public Term? StartTerm { get; set; }
}