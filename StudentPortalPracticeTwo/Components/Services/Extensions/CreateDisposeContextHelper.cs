
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public class CreateDisposeContextHelper
{

    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public CreateDisposeContextHelper(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    public async Task<T> ExecuteAsync<T>(Func<ApplicationDbContext, Task<T>> operation, ApplicationDbContext? existingContext = null)
    {
        if (existingContext != null)
        {
            return await operation(existingContext);
        }

        await using var context = await _context.CreateDbContextAsync();
        return await operation(context);
    }

    public async Task ExecuteAsync(Func<ApplicationDbContext, Task> operation, ApplicationDbContext? existingContext = null)
    {
        if (existingContext != null)
        {
            await operation(existingContext);
            return;
        }

        await using var context = await _context.CreateDbContextAsync();
        await operation(context);
    }


}

