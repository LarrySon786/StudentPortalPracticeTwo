namespace StudentPortalPracticeTwo.Database.Models.DTOs;

public class JsonStudent : JsonUserModel
{
    public string DegreeName { get; set; } = string.Empty;
    public List<JsonClassSessionReference> CurrentClassSessions { get; set; } = new();
    public List<JsonClassSessionReference> RegisteredClassSessions { get; set; } = new();
    public List<string> CompletedCourses { get; set; } = new();
}