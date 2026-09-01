

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class DegreeService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    public DegreeService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    // GET all degrees
    public async Task<List<Degree>> GetAllDegrees(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await DegreeQuery(context)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // GET degree by id
    public async Task<Degree?> GetDegreeById(int? id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            if (id == null) throw new Exception("Could not get degree by id. Id received in parameter is null");
            return await DegreeQuery(context)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // POST degree created
    public async Task CreateDegree(Degree newDegree, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            context.DegreeDb.Add(newDegree);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // PUT existing Degree
    public async Task UpdateDegree(Degree updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            Degree? existing = await GetDegreeById(updated.Id);
            if (existing == null) throw new Exception("No existing degree found. Cannot update");

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Courses = updated.Courses;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // DELETE existing Degree
    public async Task DeleteDegree(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            context.DegreeDb
                .Where(x => x.Id == id)
                .ExecuteDelete();

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    private IQueryable<Degree> DegreeQuery(ApplicationDbContext context)
    {
        return context.DegreeDb
            .Include(x => x.Courses)
                .ThenInclude(x => x.Sessions)
            .Include(x => x.StudentPrograms)
                .ThenInclude(x => x.User);
    }
}

