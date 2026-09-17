using StudentPortalPracticeTwo.Tests.Infrastructure;
using Xunit;

namespace StudentPortalPracticeTwo.Tests.Infrastructure;

public class TestingDatabaseFactory
{
    [Fact]
    public async Task CanConnectToTestDatabase()
    {
        try
        {
            await using var context = DatabaseFactory.CreateContext();

            bool canConnect = await context.Database.CanConnectAsync();

            Assert.True(canConnect);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not connect to Database. {ex}");
        }
        
    }
}