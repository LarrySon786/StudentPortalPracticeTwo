using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Database.Models.DTOs;

public class JsonFinalApplication
{
    public string Email { get; set; } = string.Empty;

    public JsonStudentInfo StudentInfo { get; set; } = new();

    public JsonStudentContact StudentContact { get; set; } = new();

    public List<JsonEmergencyContact> EmergencyContacts { get; set; } = new();

    public JsonStudentProgram StudentProgram { get; set; } = new();

    public JsonAcademicHistory AcademicHistory { get; set; } = new();

    public JsonStudentEssays Essays { get; set; } = new();

    public Status ApprovedStatus { get; set; } = Status.Pending;
}

public class JsonStudentInfo
{
    public string FirstName { get; set; } = "";
    public string MiddleName { get; set; } = "";
    public string LastName { get; set; } = "";

    public DateOnly DateOfBirth { get; set; }

    public Race Race { get; set; }

    public Gender Gender { get; set; }

    public Country CitizenshipCountry { get; set; }

    public string StreetOneAddress { get; set; } = "";

    public string? StreetTwoAddress { get; set; }

    public string City { get; set; } = "";

    public string StateOrProvince { get; set; } = "";

    public int Zipcode { get; set; }
}

public class JsonStudentContact
{
    public string Phone { get; set; } = "";

    public string? AltPhone { get; set; }
}

public class JsonEmergencyContact
{
    public string ContactName { get; set; } = "";

    public string Relationship { get; set; } = "";

    public string Phone { get; set; } = "";
}

public class JsonStudentProgram
{
    public int DegreeId { get; set; }

    public int TermId { get; set; }
}

public class JsonAcademicHistory
{
    public string HighschoolTranscriptFileName { get; set; } = "";

    // Base64 encoded in JSON
    public string HighschoolTranscript { get; set; } = "";

    public string CollegeTranscriptFileName { get; set; } = "";

    public string? CollegeTranscript { get; set; }
}

public class JsonStudentEssays
{
    public string ResponseOne { get; set; } = "";

    public string ResponseTwo { get; set; } = "";

    public string ResponseThree { get; set; } = "";
}