using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Tests.Infrastructure;
using Superpower.Model;

namespace StudentPortalPracticeTwo.Tests.Services;

public class DegreeServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly CreateDisposeContextHelper _context;
    private readonly DegreeService _degreeService;
    private readonly DatabaseFixture _fixture;

    public DegreeServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var factory = DatabaseFactory.CreateFactory();
        _context = new CreateDisposeContextHelper(factory);
        _degreeService = new DegreeService(_context);
    }

    // Get All Degrees
    [Fact]
    public async Task DegreeService_GetAllDegrees_ReturnsListOfDegrees()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Degree entityOne = new Degree()
            {
                Name = "Test Degree",
            };
            Degree entityTwo = new Degree()
            {
                Name = "Second Test",
            };

            db.DegreeDb.Add(entityOne);
            db.DegreeDb.Add(entityTwo);
            await db.SaveChangesAsync();

            // Act
            var result = await _degreeService.GetAllDegrees(db);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, x => x.Name == "Test Degree");
            Assert.Contains(result, x => x.Name == "Second Test");
        }, null);
    }

    [Fact]
    public async Task DegreeService_GetAllDegrees_ReturnsEmptyListIfNoDegreeFound()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange | Not needed

            // Act
            var result = await _degreeService.GetAllDegrees(db);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        });
    }


    // Get Degree by Id
    [Fact]
    public async Task DegreeService_GetDegreeById_ReturnsSingleDegree()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            Degree entity = new()
            {
                Id = 10,
                Name = "Test Degree"
            };
            db.DegreeDb.Add(entity);
            await db.SaveChangesAsync();

            // Act
            var result = await _degreeService.GetDegreeById(10, db);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Degree", result.Name);
        });
    }

    [Fact]
    public async Task DegreeService_GetDegreeById_NoDegreeFoundReturnNull()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange | Not needed

            // Act
            var result = await _degreeService.GetDegreeById(9999, db);

            // Assert
            Assert.Null(result);
        });
    }

    [Fact]
    public async Task DegreeService_GetDegreeById_IdParameterNullThrowException()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange | Not needed

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _degreeService.GetDegreeById(null, db));

            // Assert
            Assert.Equal("Could not get degree by id. Id received in parameter is null", exception.Message);
        });
    }


    // Create new Degree
    [Fact]
    public async Task DegreeService_CreateDegree_DegreeCreatedAndSavedInDatabase()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            var entity = new Degree()
            {
                Name = "Test Degree"
            };

            // Act
            await _degreeService.CreateDegree(entity, db);

            var result = await db.DegreeDb.FirstOrDefaultAsync(x => x.Id == entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Degree", result.Name);
        });
    }



    // Update existing degree
    [Fact]
    public async Task DegreeService_UpdateDegree_DegreeSavesUpdatedChanges()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            var entity = new Degree()
            {
                Name = "Test Degree"
            };
            db.DegreeDb.Add(entity);
            await db.SaveChangesAsync();

            // Act
            entity.Name = "Updated Degree";
            entity.Description = "I have description";
            await _degreeService.UpdateDegree(entity, db);

            var result = await db.DegreeDb.FirstOrDefaultAsync(x => x.Id == entity.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Degree", result.Name);
            Assert.Equal("I have description", result.Description);
        });
    }

    [Fact]
    public async Task DegreeService_UpdateDegree_ThrowsExceptionWhenDegreeDoesNotExist()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            var entity = new Degree()
            {
                Id = 9999,
                Name = "Test Degree"
            };

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(
                () => _degreeService.UpdateDegree(entity, db));

            // Assert
            Assert.Equal(
                "No existing degree found. Cannot update",
                exception.Message);
        });
    }

    // Delete existing degree
    [Fact]
    public async Task DegreeService_DeleteDegree_DeletesExistingDegree()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            var entity = new Degree()
            {
                Name = "Test Degree"
            };

            db.DegreeDb.Add(entity);
            await db.SaveChangesAsync();

            // Act
            await _degreeService.DeleteDegree(entity.Id, db);

            // Assert
            var result = await db.DegreeDb
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            Assert.Null(result);
        });
    }

    [Fact]
    public async Task DegreeService_DeleteDegree_DoesNothingWhenDegreeDoesNotExist()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange
            const int nonexistentId = 9999;

            // Act
            await _degreeService.DeleteDegree(nonexistentId, db);

            // Assert
            var result = await db.DegreeDb
                .FirstOrDefaultAsync(x => x.Id == nonexistentId);

            Assert.Null(result);
        });
    }
}