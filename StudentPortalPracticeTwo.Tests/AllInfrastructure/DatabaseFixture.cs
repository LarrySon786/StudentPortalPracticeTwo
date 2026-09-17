using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
namespace StudentPortalPracticeTwo.Tests.Infrastructure;

public class DatabaseFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await using var context = DatabaseFactory.CreateContext();

        await context.Database.MigrateAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var context = DatabaseFactory.CreateContext();

        await context.Database.ExecuteSqlRawAsync("""
            DO $$
            DECLARE
                table_name TEXT;
            BEGIN
                FOR table_name IN
                    SELECT tablename
                    FROM pg_tables
                    WHERE schemaname = 'public'
                        AND tablename <> '__EFMigrationsHistory'
                LOOP
                    EXECUTE 'TRUNCATE TABLE "' || table_name || '" RESTART IDENTITY CASCADE';
                END LOOP;
            END $$;
            """);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }


}