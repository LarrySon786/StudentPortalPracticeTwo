using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class DraftAcademicHistoryModel
{
    [Key]
    public int Id { get; set; }

    public int DraftApplicationId { get; set; }
    public DraftApplicationModel? DraftApplication { get; set; }

    [StringLength(255, ErrorMessage = "First name cannot exceed 255 characters")]
    public string HighschoolTranscriptFileName { get; set; } = string.Empty;
    public byte[]? HighschoolTranscript { get; set; }

    [StringLength(255, ErrorMessage = "First name cannot exceed 255 characters")]
    public string CollegeTranscriptFileName { get; set; } = string.Empty;
    public byte[]? CollegeTranscript { get; set; }
}