

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using StudentPortalPracticeTwo.Components.Ui.Application;

namespace StudentPortalPracticeTwo.Database.Models.Application;


public class DraftApplicationModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public required string Email { get; set; }

    public DraftStudentInfoModel DraftStudentInfo { get; set; } = new();

    public DraftStudentContactModel DraftStudentContact { get; set; } = new();

    public List<DraftEmergencyContactModel> DraftEmergencyContact { get; set; } = new();

    public DraftStudentProgram DraftProgramSelection { get; set; } = new();

    public DraftAcademicHistoryModel DraftAcademicHistory { get; set; } = new();

    public DraftStudentEssayModel DraftEssays { get; set; } = new();

}