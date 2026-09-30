using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class AcademicHistoryModel
{
    [Key]
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public ApplicationModel? Application { get; set; }


    [Required(ErrorMessage = "High school transcript file name is required")]
    [StringLength(255, ErrorMessage = "High school transcript file name cannot exceed 255 characters")]
    public string HighschoolTranscriptFileName { get; set; } = null!;

    [Required(ErrorMessage = "High school transcript is required")]
    public byte[] HighschoolTranscript { get; set; } = null!;

    [StringLength(255, ErrorMessage = "College transcript file name cannot exceed 255 characters")]
    public string? CollegeTranscriptFileName { get; set; }
    public byte[]? CollegeTranscript { get; set; }
}