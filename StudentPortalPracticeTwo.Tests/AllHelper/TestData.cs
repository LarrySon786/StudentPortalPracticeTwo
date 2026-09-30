
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users.Students;


namespace StudentPortalPracticeTwo.Tests.Helpers;

public static class TestData
{
    // Create Identity User
    public static ApplicationUser CreateDataIdentityUser(ApplicationDbContext db, string id = "test-id-1")
    {
        var user = new ApplicationUser()
        {
            Email = $"testUser.{id}@gmail.com",
            EmailConfirmed = true,
            Id = id,
        };

        db.Add(user);
        return user;
    }
    
    // Create Student
    public static Student CreateDataStudent(ApplicationDbContext db, string identityId)
    {
        var student = new Student()
        {
            IdentityUserId = identityId,
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

        db.Add(student);
        return student;
    }

    // Instructor
    public static Faculty CreateDataFaculty(ApplicationDbContext db, string identityId, int? facultyId = null, string? email = "test.faculty@gmail.com")
    {
        var faculty = new Faculty()
        {
            Id = facultyId ?? 1,
            FirstName = "Test",
            MiddleName = "Middle",
            LastName = "Name",
            Email = email!,
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
            IdentityUserId = identityId,
        };

        db.Add(faculty);
        return faculty;
    }

    // Create Term
    public static Term CreateDataTerm(ApplicationDbContext db, int? termId = null)
    {
        var term = new Term()
        {
            Id = termId ?? 1,
            Season = TermSeason.Fall,
            Year = 2030,
        };

        db.Add(term);
        return term;
    }

    // DEGREE, COURSE, CLASS SESSION
    // Create and return test data degree
    public static Degree CreateDataDegree(ApplicationDbContext db, int? degreeId = null)
    {
        var degree = new Degree()
        {
            Id = degreeId ?? 1,
            Name = "Test Degree",
            Description = "Test Description",
            Courses = new List<Course>()
        };

        db.Add(degree);
        return degree;
    }

    public static Course CreateDataCourse(ApplicationDbContext db, int? courseId = null)
    {
        var course = new Course()
        {
            Id = courseId ?? 1,
            Name = "Test Course",
            Code = "TST",
            Credits = 3,
        };

        db.Add(course);
        return course;
    }

    public static ClassSession CreateDataClassSession(ApplicationDbContext db, string identityId,
            int? sessionId = null, Course? course = null, Faculty? faculty = null, Term? term = null)
    {
        var session = new ClassSession()
        {
            Id = sessionId ?? 1,
            Course = course ?? CreateDataCourse(db),
            Term = term ?? CreateDataTerm(db),
            Instructor = faculty ?? CreateDataFaculty(db, identityId),
            StartDate = new DateOnly(2030, 08, 20),
            EndDate = new DateOnly(2030, 12, 14),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 15),
            Capacity = 30,
            ClassStarted = false,
            Location = "Test Location",
            Description = "Test Description",

        };

        db.Add(session);
        return session;
    }

    // Assignments
    public static Assignments CreateDataAssignment(ApplicationDbContext db, int classSessionId, int? totalPoints = null)
    {
        var assignment = new Assignments()
        {
            Name = "Test Assignment",
            Instructions = "Test Instructions",
            TotalPoints = totalPoints ?? 100,
            SessionId = classSessionId,
        };

        db.Add(assignment);
        return assignment;
    }

    // Grades
    public static Grade CreateDataGrade(ApplicationDbContext db, int assignmentId, int studentProgramId, int sessionId, int? scoredPoints = null)
    {
        var grade = new Grade()
        {
            AssignmentId = assignmentId,
            StudentProgramId = studentProgramId,
            SessionId = sessionId,
            ScoredPoints = scoredPoints ?? 0,
        };

        db.Add(grade);
        return grade;
    }

    // User program model
    public static UserProgramModel CreateDataUserProgram(ApplicationDbContext db, Student? student = null, Degree? degree = null)
    {
        UserProgramModel program = new()
        {
            User = student,
            MyDegree = degree ?? CreateDataDegree(db),
        };

        db.Add(program);
        return program;
    }
}