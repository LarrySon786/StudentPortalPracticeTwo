namespace StudentPortalPracticeTwo.Database.Models.DTOs;

public class JsonUserModel
{
    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public string Email { get; set; } = string.Empty;

    public JsonUserContact ContactDetails { get; set; } = new();

    public List<JsonUserEmergencyContact> EmergencyContact { get; set; } = new();
}