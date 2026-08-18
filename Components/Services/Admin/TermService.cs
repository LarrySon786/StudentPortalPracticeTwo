

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class TermService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public TermService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }


    // GET all terms
    public async Task<List<Term>> GetAllTerms(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existing = await context.TermDb.ToListAsync();
            return existing;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // Get Term by Id
    public async Task<Term> GetTermById(int? id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            if (id == null) throw new Exception("Could not get term by Id. Id is null");
            var existing = await context.TermDb
                .Where(x => x.Id == id)
                .Include(x => x.ClassSessions)
                .FirstOrDefaultAsync();
                if (existing != null) return existing;
                else throw new Exception("No existing term was found.");
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // Create Term
    public async Task<Term> CreateTerm(Term term, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            context.Add(term);
            await context.SaveChangesAsync();
            return term;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // Update Term
    public async Task<Term> UpdateTerm(Term updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existing = await GetTermById(updated.Id);
            if (existing == null) throw new Exception("No term found with this Id. Could not update");
            existing.Season = updated.Season;
            existing.Year = updated.Year;
            existing.ClassSessions = updated.ClassSessions;
            existing.AvailableToRegisterClasses = updated.AvailableToRegisterClasses;

            await context.SaveChangesAsync();
            return existing;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // Delete Term | Must NOT have any contigent classes
    public async Task DeleteTerm(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var existing = await GetTermById(id);
            // Verify that NO Class sessions remain in the term
            if (existing.ClassSessions.Count > 0) throw new Exception("Cannot delete this term as long as it has active class sessions attached to it");
            // Delete Term
            else await context.TermDb.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }

    }

}