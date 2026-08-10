using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class DraftStudentEssayModel
{
    [Key]
    public int Id { get; set; }

    public DraftApplicationModel? DraftApplication { get; set; }
    public int DraftApplicationId { get; set; }

    public string ResponseOne { get; set; } = string.Empty;
    public string ResponseTwo { get; set; } = string.Empty;
    public string ResponseThree { get; set; } = string.Empty;
}