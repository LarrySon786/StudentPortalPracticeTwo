
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Database.Models.DTOs;

public class JsonClassSessions
{
    public string CourseCode { get; set; } = null!;
    public string Instructor { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Term { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int CurrentCount { get; set; }
    public int Capacity { get; set; }
}

