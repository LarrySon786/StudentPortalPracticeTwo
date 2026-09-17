using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;
using StudentPortalPracticeTwo.Tests.Infrastructure;

namespace StudentPortalPracticeTwo.Tests.Services;

public class TermServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly CreateDisposeContextHelper _context;
    private readonly TermService _termService;
    private readonly DatabaseFixture _fixture;

    public TermServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var factory = DatabaseFactory.CreateFactory();
        _context = new CreateDisposeContextHelper(factory);
        _termService = new TermService(_context);
    }


    // TEST display name
    [Fact]
    public void TermModel_DisplayName_FunctionsAsExpected_DisplaysName()
    {
        // Arrange
        Term term = new()
        {
            Year = 2027,
            AvailableToRegisterClasses = true,
            Season = TermSeason.Fall,
        };

        // Act
        string result = term.DisplayName;

        // Assert
        Assert.Equal("Fall 2027", result);
    }


    // TEST GET ALL TERMS
    [Fact]
    public async Task TermService_GetAllTerms_ReturnsAllTerms()
    {
        await _fixture.ResetDatabaseAsync();

        // Arrange
        await _context.ExecuteAsync(async db =>
        {
            var termOne = new Term()
            {
                AvailableToRegisterClasses = true,
                Season = TermSeason.Fall,
                Year = 2026,
            };
            var termTwo = new Term()
            {
                AvailableToRegisterClasses = true,
                Season = TermSeason.Spring,
                Year = 2027,
            };

            db.TermDb.Add(termOne);
            db.TermDb.Add(termTwo);
            await db.SaveChangesAsync();

            // Act
            List<Term> terms = await _termService.GetAllTerms();

            // Assert
            Assert.Equal(2, terms.Count);
            Assert.Contains(terms, t =>
                t.Season == TermSeason.Fall &&
                t.Year == 2026);
            Assert.Contains(terms, t =>
                t.Season == TermSeason.Spring &&
                t.Year == 2027);
        }, null);
    }

    [Fact]
    public async Task TermService_GetAllTerms_ReturnsEmptyListWhenCountIsZero()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // Arrange | Not Needed
            // Act
            List<Term> terms = await _termService.GetAllTerms();

            // Assert
            Assert.NotNull(terms);
            Assert.Empty(terms);
        }, null);
    }

    // TEST GET Term by Id
    [Fact]
    public async Task TermService_GetTermById_ReturnsOneTerm()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        // Act
        await _context.ExecuteAsync(async db =>
        {
            Term entity = new()
            {
                Season = TermSeason.Fall,
                Year = 2032,
                AvailableToRegisterClasses = false,
            };

            var result = await _termService.CreateTerm(entity, db);

            var finalEntity = await _termService.GetTermById(result.Id, db);

            // Assert
            Assert.Equal(entity.Season, finalEntity.Season);
            Assert.Equal(entity.AvailableToRegisterClasses, finalEntity.AvailableToRegisterClasses);
            Assert.Equal(entity.Year, finalEntity.Year);
        }, null);
    }

    [Fact]
    public async Task TermService_GetTermById_ThrowsExceptionWhenIdDoesNotExist()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        // Act
        await _context.ExecuteAsync(async db =>
        {
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _termService.GetTermById(9999, db)
            );

            // Assert
            Assert.Equal("No existing term was found.", exception.Message);

        }, null);
    }

    [Fact]
    public async Task TermService_GetTermById_ThrowsExceptionWhenIdIsNull()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        // Act
        await _context.ExecuteAsync(async db =>
        {
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _termService.GetTermById(null, db)
            );

            // Assert
            Assert.Equal("Could not get term by Id. Id is null", exception.Message);

        }, null);
    }

    // TEST CREATE Term
    [Fact]
    public async Task TermService_CreateTerm_PersistsTerm()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        // Act
        await _context.ExecuteAsync(async db =>
        {
            Term entity = new()
            {
                Season = TermSeason.Spring,
                Year = 2030,
                AvailableToRegisterClasses = false,
            };

            var created = await _termService.CreateTerm(entity, db);

            var result = await db.TermDb.Where(x => x.Id == created.Id).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Season, result.Season);
            Assert.Equal(entity.Year, result.Year);
            Assert.Equal(entity.AvailableToRegisterClasses, result.AvailableToRegisterClasses);

        }, null);
    }


    // TEST UPDATE Term
    [Fact]
    public async Task TermService_UpdateTerm_PersistsTermUpdates()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // ARRANGE
            Term entity = new()
            {
                Season = TermSeason.Spring,
                Year = 2030,
                AvailableToRegisterClasses = false,
            };

            db.TermDb.Add(entity);
            await db.SaveChangesAsync();

            // ACT

            entity.Year = 2031;
            entity.AvailableToRegisterClasses = true;
            entity.Season = TermSeason.Fall;

            await _termService.UpdateTerm(entity, db);

            var result = await db.TermDb.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Season, result.Season);
            Assert.Equal(entity.Year, result.Year);
            Assert.Equal(entity.AvailableToRegisterClasses, result.AvailableToRegisterClasses);

        }, null);
    }

    [Fact]
    public async Task TermService_UpdateTerm_ThrowsErrorWhenNoEntityFoundById()
    {
        // Arrange
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // ARRANGE
            Term entity = new()
            {
                Id = 999999,
                Season = TermSeason.Spring,
                Year = 2030,
                AvailableToRegisterClasses = false,
            };
            // Intentially do not save this to the database

            // ACT
            var exception = await Assert.ThrowsAsync<Exception>(() => _termService.UpdateTerm(entity, db));

            // Assert
            Assert.Equal("No existing term was found.", exception.Message);
        }, null);
    }

    // TEST DELETE Term
    [Fact]
    public async Task TermService_DeleteTerm_DeletionPersists()
    {
        await _fixture.ResetDatabaseAsync();

        await _context.ExecuteAsync(async db =>
        {
            // ARRANGE
            Term entity = new()
            {
                Season = TermSeason.Spring,
                Year = 2040,
                AvailableToRegisterClasses = false,
            };

            db.TermDb.Add(entity);
            await db.SaveChangesAsync();

            // ACT
            await _termService.DeleteTerm(entity.Id, db);
            var result = await db.TermDb.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            // ASSERT
            Assert.Null(result);
        }, null);
    }

    [Fact]
    public async Task TermService_DeleeteTerm_CannotDeleteWhenClassSessionExistsInTerm() {
        await _context.ExecuteAsync(async db => {
            // Arrange
            Term entity = new()
            {
                Season = TermSeason.Spring,
                Year = 2030,
                AvailableToRegisterClasses = false,
                ClassSessions = [new() {
                    Description = "none",
                    Course = new() {
                        Name = "Test",
                        Credits = 3,
                        Code = "TST",
                    },
                    Instructor = new() {
                        FirstName = "First",
                        LastName = "Last",
                        Email = "test@gmail.com",
                        DateOfBirth = new DateOnly(1990, 1, 1),
                        ContactDetails = new() {
                            Phone = "444-555-6666"
                        },
                        IdentityUser = new(),
                        IdentityUserId = "999",
                        EmergencyContact = [new() {
                            ContactName = "TestMom",
                            Phone = "111-222-3333",
                            Relationship = "Mother",
                        }]
                    },
                }]
            };

            db.TermDb.Add(entity);
            await db.SaveChangesAsync();

            // ACT
            var exception = await Assert.ThrowsAsync<Exception>(() => _termService.DeleteTerm(entity.Id, db));

            Assert.Equal("Cannot delete this term as long as it has active class sessions attached to it", exception.Message);
        }, null);
    }
}

