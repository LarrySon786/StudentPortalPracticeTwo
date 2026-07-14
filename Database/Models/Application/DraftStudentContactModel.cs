

using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;


public class DraftStudentContactModel
{
    public int Id { get; set; }

    public DraftApplicationModel Application { get; set; } = null!;

    [Phone(ErrorMessage = "Must be a valid phone number")]
    public string? Phone { get; set; }


}