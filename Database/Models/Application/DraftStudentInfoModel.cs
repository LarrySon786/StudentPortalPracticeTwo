

using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;


public class DraftStudentInfoModel
{
    public int Id { get; set; }

    public DraftApplicationModel Application { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}