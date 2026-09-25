using System.Security.Cryptography.X509Certificates;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Pages.Dashboard;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Students;
using StudentPortalPracticeTwo.Tests.Helpers;
using StudentPortalPracticeTwo.Tests.Infrastructure;

namespace StudentPortalPracticeTwo.Tests.Services;

public class ClassSessionServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly CreateDisposeContextHelper _context;
    private readonly ClassSessionService _classSessionService;
    private readonly DatabaseFixture _fixture;

    public ClassSessionServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var factory = DatabaseFactory.CreateFactory();
        _context = new CreateDisposeContextHelper(factory);
        _classSessionService = new ClassSessionService(_context);
    }

    // ============================================================
    // GET ALL CLASS SESSIONS
    // ============================================================

    [Fact]
    public async Task ClassSessionService_GetAllClassSessions_ReturnsEmptyListWhenNoSessionsExist()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Act
            var result = await _classSessionService.GetAllClassSessions(db);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        });
    }

    [Fact]
    public async Task ClassSessionService_GetAllClassSessions_ReturnsAllClassSessions()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            ApplicationUser facultyOne = TestData.CreateDataIdentityUser("faculty-one-id");

            ClassSession sessionOne = new ClassSession
            {
                Location = "Room 101",
                Description = "Mathematics Class",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                CurrentCount = 10,
                Capacity = 30,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyOne.Id),

            };

            ApplicationUser facultyTwo = TestData.CreateDataIdentityUser("faculty-two-id");

            ClassSession sessionTwo = new ClassSession
            {
                Location = "Room 202",
                Description = "Science Class",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(11, 0),
                EndTime = new TimeOnly(12, 0),
                CurrentCount = 15,
                Capacity = 25,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyTwo.Id),
            };
            db.Users.AddRange(facultyOne, facultyTwo);
            db.ClassSessionDb.AddRange(sessionOne, sessionTwo);
            await db.SaveChangesAsync();

            // Act
            var result = await _classSessionService.GetAllClassSessions(db);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Location == "Room 101");
            Assert.Contains(result, x => x.Location == "Room 202");
        });
    }


    // ============================================================
    // GET CLASS SESSION BY ID
    // ============================================================

    [Fact]
    public async Task ClassSessionService_GetClassSessionById_ReturnsSession()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            var facultyOne = TestData.CreateDataIdentityUser();

            // Arrange
            ClassSession session = new ClassSession
            {
                Location = "Room 101",
                Description = "Test Class",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                CurrentCount = 5,
                Capacity = 20,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyOne.Id),
            };

            db.Users.Add(facultyOne);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            // Act
            var result = await _classSessionService
                .GetClassSessionById(session.Id, db);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(session.Id, result.Id);
            Assert.Equal("Room 101", result.Location);
            Assert.Equal("Test Class", result.Description);
            Assert.Equal(5, result.CurrentCount);
            Assert.Equal(20, result.Capacity);
        });
    }

    [Fact]
    public async Task ClassSessionService_GetClassSessionById_ReturnsNullIfSessionDoesNotExist()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            const int nonExistentId = 999999;

            // Act
            var result = await _classSessionService
                .GetClassSessionById(nonExistentId, db);

            // Assert
            Assert.Null(result);
        });
    }


    // ============================================================
    // CREATE CLASS SESSION
    // ============================================================

    [Fact]
    public async Task ClassSessionService_CreateClassSession_CreatesAndSavesSession()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            var facultyOne = TestData.CreateDataIdentityUser();

            // Arrange
            ClassSession session = new ClassSession
            {
                Location = "Room 301",
                Description = "New Test Class",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(14, 0),
                CurrentCount = 0,
                Capacity = 25,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyOne.Id),
            };

            db.Users.Add(facultyOne);

            // Act
            var result = await _classSessionService
                .CreateClassSession(session, db);

            // Assert - returned entity
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("Room 301", result.Location);
            Assert.Equal("New Test Class", result.Description);

            // Assert - persisted entity
            var databaseResult = await db.ClassSessionDb
                .FirstOrDefaultAsync(x => x.Id == result.Id);

            Assert.NotNull(databaseResult);
            Assert.Equal("Room 301", databaseResult.Location);
            Assert.Equal("New Test Class", databaseResult.Description);
            Assert.Equal(25, databaseResult.Capacity);
        });
    }


    // ============================================================
    // UPDATE CLASS SESSION
    // ============================================================

    [Fact]
    public async Task ClassSessionService_UpdateClassSession_ChangesAndSavesSession()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            var facultyOne = TestData.CreateDataIdentityUser();

            // Arrange
            ClassSession existingSession = new ClassSession
            {
                Location = "Original Room",
                Description = "Original Description",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                CurrentCount = 5,
                Capacity = 20,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyOne.Id),
            };

            db.Users.Add(facultyOne);
            db.ClassSessionDb.Add(existingSession);
            await db.SaveChangesAsync();

            ClassSession updatedSession = new ClassSession
            {
                Id = existingSession.Id,
                Location = "Updated Room",
                Description = "Updated Description",
                StartDate = new DateOnly(2026, 10, 1),
                EndDate = new DateOnly(2027, 1, 15),
                StartTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(14, 30),
                CurrentCount = 10,
                Capacity = 30,
                CourseId = existingSession.CourseId,
                TermId = existingSession.TermId,
                InstructorId = existingSession.InstructorId,
            };

            // Act
            await _classSessionService.UpdateClassSession(updatedSession, db);

            // Assert
            var result = await db.ClassSessionDb
                .FirstOrDefaultAsync(x => x.Id == existingSession.Id);

            Assert.NotNull(result);

            Assert.Equal("Updated Room", result.Location);
            Assert.Equal("Updated Description", result.Description);
            Assert.Equal(new DateOnly(2026, 10, 1), result.StartDate);
            Assert.Equal(new DateOnly(2027, 1, 15), result.EndDate);
            Assert.Equal(new TimeOnly(13, 0), result.StartTime);
            Assert.Equal(new TimeOnly(14, 30), result.EndTime);
            Assert.Equal(10, result.CurrentCount);
            Assert.Equal(30, result.Capacity);
        });
    }

    [Fact]
    public async Task ClassSessionService_UpdateClassSession_ThrowsIfSessionDoesNotExist()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            ClassSession session = new ClassSession
            {
                Id = 999999,
                Location = "Does Not Exist",
                Description = "This session should not exist",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                CurrentCount = 0,
                Capacity = 20
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _classSessionService.UpdateClassSession(session, db));

            Assert.Equal(
                "No existing class session was found. Update failed",
                exception.Message);
        });
    }


    // ============================================================
    // DELETE CLASS SESSION
    // ============================================================

    [Fact]
    public async Task ClassSessionService_DeleteClassSession_DeletesExistingSession()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            var facultyOne = TestData.CreateDataIdentityUser();
            // Arrange
            ClassSession session = new ClassSession
            {
                Location = "Room To Delete",
                Description = "Delete Test",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                CurrentCount = 0,
                Capacity = 20,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyOne.Id),
            };

            db.Users.Add(facultyOne);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            int sessionId = session.Id;

            // Act
            await _classSessionService
                .DeleteClassSession(sessionId, db);

            // Assert
            var result = await db.ClassSessionDb
                .FirstOrDefaultAsync(x => x.Id == sessionId);

            Assert.Null(result);
        });
    }

    [Fact]
    public async Task ClassSessionService_DeleteClassSession_ThrowsIfSessionDoesNotExist()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            const int nonExistentId = 999999;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _classSessionService
                    .DeleteClassSession(nonExistentId, db));

            Assert.Equal(
                "Could not delete class session. No matching id found.",
                exception.Message);
        });
    }
    // ARCHIVE class sessions
    [Fact]
    public async Task ClassSessionService_ArchiveAndCloseClassSession_AssertSessionCloses()
    {
        await _fixture.ResetDatabaseAsync();
        await _context.ExecuteAsync(async db =>
        {
            var facultyOne = TestData.CreateDataIdentityUser();
            // Arrange
            ClassSession session = new ClassSession
            {
                Location = "Room To Archive",
                Description = "Archive Test",
                StartDate = new DateOnly(2026, 9, 1),
                EndDate = new DateOnly(2026, 12, 15),
                StartTime = new TimeOnly(9, 0),
                EndTime = new TimeOnly(10, 0),
                CurrentCount = 0,
                Capacity = 20,
                Course = TestData.CreateDataCourse(),
                Term = TestData.CreateDataTerm(),
                Instructor = TestData.CreateDataFaculty(facultyOne.Id),
            };
            db.Users.Add(facultyOne);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            // Act
            await _classSessionService.ArchiveAndCloseClassSession(session.Id, db);

            var result = await db.ClassSessionDb.FirstOrDefaultAsync(x => x.Id == session.Id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ArchivedAndClosed);
        });
    }

    [Fact]
    public async Task ClassSessionService_ArchiveAndCloseClassSession_AssertExceptionThrownIfRegisteredStudents()
    {
        await _fixture.ResetDatabaseAsync();
        await _context.ExecuteAsync(async db =>
        {
            // Arrange | Create a registered student 
            ApplicationUser faculty = TestData.CreateDataIdentityUser("instructor-id");
            ClassSession session = TestData.CreateDataClassSession(faculty.Id);
            ApplicationUser user = TestData.CreateDataIdentityUser("test-student-1");
            Student student = TestData.CreateDataStudent(user.Id);
            UserProgramModel program = TestData.CreateDataUserProgram(student);
            program.RegisteredSessions.Add(session);
            session.RegisteredStudentProgramModels.Add(program);

            db.Users.AddRange(user, faculty);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() => _classSessionService.ArchiveAndCloseClassSession(session.Id, db));
            // Assert
            Assert.Equal("Could not archive class session. Some students are registered in this session", exception.Message);
        });
    }

    [Fact]
    public async Task ClassSessionService_ArchiveAndCloseClassSession_AssertExceptionThrownIfCurrentlyActiveStudent()
    {
        await _fixture.ResetDatabaseAsync();
        await _context.ExecuteAsync(async db =>
        {
            // Arrange | Create a registered student 
            ApplicationUser faculty = TestData.CreateDataIdentityUser("instructor-id");
            ClassSession session = TestData.CreateDataClassSession(faculty.Id);
            ApplicationUser user = TestData.CreateDataIdentityUser("test-student-1");
            Student student = TestData.CreateDataStudent(user.Id);
            UserProgramModel program = TestData.CreateDataUserProgram(student);
            program.CurrentSessions.Add(session);
            session.StudentProgramModels.Add(program);

            db.Users.AddRange(faculty, user);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            // Act
            Exception exception = await Assert.ThrowsAsync<Exception>(() => _classSessionService.ArchiveAndCloseClassSession(session.Id, db));
            // Assert
            Assert.Equal("Could not archive class session. Some students are still enrolled in this session", exception.Message);
        });
    }


    // Unarchive a session
    [Fact]
    public async Task ClassSessionService_UnArchiveAndOpenClassSession_SucceedsToOpenClass()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            var faculty = TestData.CreateDataIdentityUser();
            var session = TestData.CreateDataClassSession(faculty.Id);

            db.Users.Add(faculty);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            // Act
            await _classSessionService.UnArchiveAndOpenClassSession(session.Id, db);
            var result = await db.ClassSessionDb.FirstOrDefaultAsync(x => x.Id == session.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.ArchivedAndClosed);

        });
    }


    [Fact]
    public async Task ClassSessionService_UnArchiveAndOpenClassSession_ThrowsIfSessionIsNotFound()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            const int id = 99999;

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _classSessionService.UnArchiveAndOpenClassSession(id, db));

            // Assert
            Assert.Equal("Could not find an existing class session to archive", exception.Message);

        });
    }

    // **********************
    // Query Test
    // **********************

    [Fact]
    public async Task ClassSessionService_QueryTests_QueryReturnsAllValues()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            ApplicationUser faculty = TestData.CreateDataIdentityUser("test-id");
            ClassSession session = TestData.CreateDataClassSession(faculty.Id);
            Assignments assignment = TestData.CreateDataAssignment(session.Id);
            ApplicationUser user = TestData.CreateDataIdentityUser("test-student-id");
            Student student = TestData.CreateDataStudent(user.Id);
            Degree degree = TestData.CreateDataDegree();
            UserProgramModel program = TestData.CreateDataUserProgram(student, degree);
            Grade grade = TestData.CreateDataGrade(assignment.Id, program.Id, session.Id);
            assignment.Grades.Add(grade);
            program.Grade.Add(grade);
            session.Assignments.Add(assignment);
            student.MyProgram = program;
            session.StudentProgramModels.Add(program);
            session.RegisteredStudentProgramModels.Add(program);

            db.Users.AddRange(faculty, user);
            db.StudentDb.Add(student);
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();

            // Act
            var result = await _classSessionService.GetClassSessionById(session.Id, db);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Instructor);
            Assert.Equal("test.faculty@gmail.com", result.Instructor.Email);
            Assert.NotNull(result.Course);
            Assert.Equal("TST", result.Course.Code);
            Assert.NotNull(result.Assignments);
            Assert.NotEmpty(result.Assignments);
            Assert.NotNull(result.StudentProgramModels);
            Assert.NotEmpty(result.StudentProgramModels);
            Assert.NotNull(result.Term);
            Assert.Equal(2030, result.Term.Year);
        });
    }


}

