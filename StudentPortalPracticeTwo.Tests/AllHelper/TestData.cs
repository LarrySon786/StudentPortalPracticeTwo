
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users.Students;


namespace StudentPortalPracticeTwo.Tests.Helpers;

public static class TestData
{
    // Create Identity User
    public static ApplicationUser CreateDataIdentityUser(string id = "test-id-1")
    {
        var user = new ApplicationUser()
        {
            Email = $"testUser.{id}@gmail.com",
            EmailConfirmed = true,
            Id = id,
        };
        return user;
    }
    
    // Create Student
    public static Student CreateDataStudent(string? identityId = null)
    {
        return new Student()
        {
            IdentityUserId = identityId ?? "student-test-id",
            FirstName = "Test",
            MiddleName = "Middle",
            LastName = "Student",
            Email = "test.student@gmail.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            ContactDetails = new()
            {
                Phone = "111-222-3333"
            },
            EmergencyContact = [
                new UserEmergencyContactModel() {
                    ContactName = "Test",
                    Relationship = "Test Relationship",
                    Phone = "555-666-7777"
                }
            ],
            MyProgram = new(),
        };
    }

    // Instructor
    public static Faculty CreateDataFaculty(string? identityId = null)
    {
        var user = CreateDataIdentityUser();

        return new()
        {
            FirstName = "Test",
            MiddleName = "Middle",
            LastName = "Name",
            Email = "test.faculty@gmail.com",
            DateOfBirth = new DateOnly(2000, 1, 1),
            ContactDetails = new()
            {
                Phone = "111-222-3333"
            },
            EmergencyContact = [
                new UserEmergencyContactModel() {
                    ContactName = "Test",
                    Relationship = "Test Relationship",
                    Phone = "555-666-7777"
                }
            ],
            IdentityUserId = identityId ?? "faculty-test-id",
        };
    }

    // Create Term
    public static Term CreateDataTerm()
    {
        return new Term()
        {
            Season = TermSeason.Fall,
            Year = 2030,
        };
    }

    // DEGREE, COURSE, CLASS SESSION
    // Create and return test data degree
    public static Degree CreateDataDegree()
    {
        var degree = new Degree()
        {
            Name = "Test Degree",
            Description = "Test Description",
            Courses = new List<Course>()
        };

        return degree;
    }

    public static Course CreateDataCourse()
    {
        var course = new Course()
        {
            Name = "Test Course",
            Code = "TST",
            Credits = 3,
        };

        return course;
    }

    public static ClassSession CreateDataClassSession( string identityId, int? sessionId = null, Course? course = null)
    {
        var session = new ClassSession()
        {
            Id = sessionId ?? 1,
            Course = course ?? CreateDataCourse(),
            Term = CreateDataTerm(),
            Instructor = CreateDataFaculty(identityId),
            StartDate = new DateOnly(2030, 08, 20),
            EndDate = new DateOnly(2030, 12, 14),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 15),
            Capacity = 30,
            ClassStarted = false,
            Location = "Test Location",
            Description = "Test Description",

        };

        return session;
    }

    // Assignments
    public static Assignments CreateDataAssignment(int classSessionId, int? totalPoints = null)
    {
        return new Assignments()
        {
            Name = "Test Assignment",
            Instructions = "Test Instructions",
            TotalPoints = totalPoints ?? 100,
            SessionId = classSessionId,
        };
    }

    // Grades
    public static Grade CreateDataGrade(int assignmentId, int studentProgramId, int sessionId, int? scoredPoints = null)
    {
        return new Grade()
        {
            AssignmentId = assignmentId,
            StudentProgramId = studentProgramId,
            SessionId = sessionId,
            ScoredPoints = scoredPoints ?? 0,
        };
    }

    // User program model
    public static UserProgramModel CreateDataUserProgram(Student? student = null, Degree? degree = null)
    {
        UserProgramModel program = new()
        {
            User = student,
            MyDegree = degree ?? CreateDataDegree(),
        };

        return program;
    }
}