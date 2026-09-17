using System.ComponentModel.DataAnnotations;

namespace StudentPortalPracticeTwo.Database.Models.Application;

public class AcademicHistoryModel
{
    [Key]
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public ApplicationModel? Application { get; set; }


    public string HighschoolTranscriptFileName { get; set; } = null!;
    public byte[] HighschoolTranscript { get; set; } = null!;

    public string CollegeTranscriptFileName { get; set; } = null!;
    public byte[]? CollegeTranscript { get; set; }
}