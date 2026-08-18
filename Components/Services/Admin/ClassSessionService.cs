
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class ClassSessionService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public ClassSessionService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    // GET all class sessions (order by future to past)
    public async Task<List<ClassSession>> GetAllClassSessions(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.ClassSessionDb
                .Include(x => x.Course)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // GET class session by Id
    public async Task<ClassSession?> GetClassSessionById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            return await context.ClassSessionDb
            .Where(x => x.Id == id)
            .Include(x => x.Course)
            .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // POST class session
    public async Task CreateClassSession(ClassSession session, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            context.ClassSessionDb.Add(session);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // PUT class session
    public async Task UpdateClassSession(ClassSession updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            ClassSession? existing = await GetClassSessionById(updated.Id, context);
            if (existing == null) throw new Exception("No existing class session was found. Update failed");

            existing.Instructor = updated.Instructor;
            existing.Capacity = updated.Capacity;
            existing.CurrentCount = updated.CurrentCount;
            existing.Description = updated.Description;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.Location = updated.Location;
            existing.Term!.Id = updated.Term!.Id;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // DELETE existing class session
    public async Task DeleteClassSession(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            await context.ClassSessionDb
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }
}