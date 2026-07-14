
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class ClassSessionService
{
    private readonly ApplicationDbContext _context;

    public ClassSessionService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET all class sessions (order by future to past)
    public async Task<List<ClassSession>> GetAllClassSessions()
    {
        return await _context.ClassSessionDb
            .Include(x => x.Course)
            .ToListAsync();
    }

    // GET class session by Id
    public async Task<ClassSession?> GetClassSessionById(int id)
    {
        return await _context.ClassSessionDb
            .Where(x => x.Id == id)
            .Include(x => x.Course)
            .FirstOrDefaultAsync();
    }

    // POST class session
    public async Task CreateClassSession(ClassSession session)
    {
        _context.ClassSessionDb.Add(session);
        await _context.SaveChangesAsync();
    }

    // PUT class session
    public async Task UpdateClassSession(ClassSession updated)
    {
        ClassSession? existing = await GetClassSessionById(updated.Id);
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

        await _context.SaveChangesAsync();
    }

    // DELETE existing class session
    public async Task DeleteClassSession(int id)
    {
        _context.ClassSessionDb
            .Where(x => x.Id == id)
            .ExecuteDelete();
    }
}