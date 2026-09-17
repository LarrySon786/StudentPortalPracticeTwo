using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StudentPortalPracticeTwo.Tests.Infrastructure;

public static class DatabaseFactory
{
    private static readonly IConfiguration Configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.Test.json")
        .AddUserSecrets<TestProjectMarker>(optional: true)
        .Build();

    private static readonly string ConnectionString =
        Configuration.GetConnectionString("TestingConnectionString")
        ?? throw new InvalidOperationException(
            "Test database connection string was not found");

    // Do not use the Create Context for normal tests. Default to CreateFactory()
    public static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    public static IDbContextFactory<ApplicationDbContext> CreateFactory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new PooledDbContextFactory<ApplicationDbContext>(options);
    }
}

public class TestProjectMarker
{
}