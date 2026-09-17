namespace StudentPortalPracticeTwo.Database.Models.DTOs;

public class JsonDegree
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Courses { get; set; } = [];
}