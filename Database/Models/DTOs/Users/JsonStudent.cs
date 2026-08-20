namespace StudentPortalPracticeTwo.Database.Models.DTOs;

public class JsonStudent : JsonUserModel
{
    public string DegreeName { get; set; } = string.Empty; // The student's Degree Name
}

