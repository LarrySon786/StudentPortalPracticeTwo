using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Tests.Infrastructure;

namespace StudentPortalPracticeTwo.Tests.Services;

public class CourseServiceTest : IClassFixture<DatabaseFixture>
{
    private readonly CreateDisposeContextHelper _context;
    private readonly CourseService _courseService;
    private readonly DatabaseFixture _fixture;

    public CourseServiceTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var factory = DatabaseFactory.CreateFactory();
        _context = new CreateDisposeContextHelper(factory);
        _courseService = new CourseService(_context);
    }

    // Get All Courses
    [Fact]
    public async Task CourseService_GetAllCourses_ReturnsListOfCourses()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Course entityOne = new Course()
            {
                Name = "Mathmatics Test",
                Code = "MATH123",
                Credits = 1
            };

            Course entityTwo = new Course()
            {
                Name = "Science Test",
                Code = "PHY123",
                Credits = 2
            };

            db.CourseDb.Add(entityOne);
            db.CourseDb.Add(entityTwo);
            await db.SaveChangesAsync();

            // Act
            var result = await _courseService.GetAllCourses(db);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Name == "Mathmatics Test");
            Assert.Contains(result, x => x.Name == "Science Test");
        });
    }

    // GET BY ID
    [Fact]
    public async Task CourseService_GetCourseById_ReturnsSingleCourse()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Course course = new Course()
            {
                Name = "Mathematics Test",
                Code = "MATH123",
                Credits = 3
            };

            db.CourseDb.Add(course);
            await db.SaveChangesAsync();

            // Act
            var result = await _courseService.GetCourseById(course.Id, db);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(course.Id, result.Id);
            Assert.Equal("Mathematics Test", result.Name);
            Assert.Equal("MATH123", result.Code);
            Assert.Equal(3, result.Credits);
        });
    }

    [Fact]
    public async Task CourseService_GetCourseById_ReturnsNullIfNoCourseFound()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            const int nonExistentId = 999999;

            // Act
            var result = await _courseService.GetCourseById(nonExistentId, db);

            // Assert
            Assert.Null(result);
        });
    }

    // Create Course
    [Fact]
    public async Task CourseService_CreateCourse_CreateAndSaveCourseInDatabase()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Course course = new Course()
            {
                Name = "Computer Science Test",
                Code = "CS123",
                Credits = 4
            };

            // Act
            await _courseService.CreateCourse(course, db);

            // Assert
            var result = await db.CourseDb
                .FirstOrDefaultAsync(x => x.Code == "CS123");

            Assert.NotNull(result);
            Assert.Equal("Computer Science Test", result.Name);
            Assert.Equal("CS123", result.Code);
            Assert.Equal(4, result.Credits);
        });
    }

    // PUT course
    [Fact]
    public async Task CourseService_UpdateCourse_ChangesSaveAndStoreInDatabase()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Course existingCourse = new Course()
            {
                Name = "Original Course",
                Code = "OLD123",
                Credits = 2
            };

            db.CourseDb.Add(existingCourse);
            await db.SaveChangesAsync();

            Course updatedCourse = new Course()
            {
                Id = existingCourse.Id,
                Name = "Updated Course",
                Code = "NEW123",
                Credits = 4
            };

            // Act
            await _courseService.UpdateCourse(updatedCourse, db);

            // Assert
            var result = await db.CourseDb
                .FirstOrDefaultAsync(x => x.Id == existingCourse.Id);

            Assert.NotNull(result);
            Assert.Equal(existingCourse.Id, result.Id);
            Assert.Equal("Updated Course", result.Name);
            Assert.Equal("NEW123", result.Code);
            Assert.Equal(4, result.Credits);
        });
    }

    [Fact]
    public async Task CourseService_UpdateCourse_CourseNotFoundWithThatId()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Course course = new Course()
            {
                Id = 999999,
                Name = "Does Not Exist",
                Code = "NONE123",
                Credits = 3
            };

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _courseService.UpdateCourse(course, db));

            // Assert
            Assert.Equal("No existing course found. Updating course failed.", exception.Message);
        });
    }

    // Delete Course
    [Fact]
    public async Task CourseService_DeleteCourse_CourseSuccessfullyDeletes()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Course course = new Course()
            {
                Name = "Course To Delete",
                Code = "DEL123",
                Credits = 3
            };

            db.CourseDb.Add(course);
            await db.SaveChangesAsync();

            int courseId = course.Id;

            // Act
            await _courseService.DeleteCourse(courseId, db);

            // Assert
            var result = await db.CourseDb
                .FirstOrDefaultAsync(x => x.Id == courseId);

            Assert.Null(result);
        });
    }

    [Fact]
    public async Task CourseService_DeleteCourse_DoesNothingIfNoCourseFound()
    {
        await _fixture.ResetDatabaseAsync();
        
        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            const int nonExistentId = 999999;

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _courseService.DeleteCourse(nonExistentId, db));

            // Assert
            Assert.Equal("No course found to delete. Delete failed.", exception.Message);

            var result = await db.CourseDb
                .FirstOrDefaultAsync(x => x.Id == nonExistentId);

            Assert.Null(result);
        });
    }
}

