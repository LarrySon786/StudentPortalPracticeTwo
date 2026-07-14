

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class DegreeService
{
    private readonly ApplicationDbContext _context;
    public DegreeService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET all degrees
    public async Task<List<Degree>> GetAllDegrees()
    {
        return await _context.DegreeDb
            .Include(x => x.Courses)
            .ToListAsync();
    }

    // GET degree by id
    public async Task<Degree?> GetDegreeById(int id)
    {
        return await _context.DegreeDb
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    // POST degree created
    public async Task CreateDegree(Degree newDegree)
    {
        _context.DegreeDb.Add(newDegree);
        await _context.SaveChangesAsync();
    }

    // PUT existing Degree
    public async Task UpdateDegree(Degree updated)
    {
        Degree? existing = await GetDegreeById(updated.Id);
        if (existing == null) throw new Exception("No existing degree found. Cannot update");

        existing.Name = updated.Name;
        existing.Description = updated.Description;
        existing.Courses = updated.Courses;

        await _context.SaveChangesAsync();
    }

    // DELETE existing Degree
    public async Task DeleteDegree(int id)
    {
        _context.DegreeDb
            .Where(x => x.Id == id)
            .ExecuteDelete();

        await _context.SaveChangesAsync();
    }
}

