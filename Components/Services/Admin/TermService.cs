

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class TermService
{
    private readonly ApplicationDbContext _context;

    public TermService(ApplicationDbContext context)
    {
        _context = context;
    }


    // GET all terms
    public async Task<List<Term>> GetAllTerms()
    {
        Console.WriteLine(_context.GetHashCode());
        var existing = await _context.TermDb.ToListAsync();
        return existing;
    }

    // Get Term by Id
    public async Task<Term> GetTermById(int? id)
    {
        if (id == null) throw new Exception("Could not get term by Id. Id is null");
        var existing = await _context.TermDb
            .Where(x => x.Id == id)
            .Include(x => x.ClassSessions)
            .FirstOrDefaultAsync();
        if (existing != null) return existing;
        else throw new Exception("No existing term was found.");
    }

    // Create Term
    public async Task<Term> CreateTerm(Term term)
    {
        _context.Add(term);
        await _context.SaveChangesAsync();
        return term;
    }

    // Update Term
    public async Task<Term> UpdateTerm(Term updated)
    {
        var existing = await GetTermById(updated.Id);
        if (existing == null) throw new Exception("No term found with this Id. Could not update");
        existing.Season = updated.Season;
        existing.Year = updated.Year;
        existing.ClassSessions = updated.ClassSessions;
        existing.AvailableToRegisterClasses = updated.AvailableToRegisterClasses;

        await _context.SaveChangesAsync();
        return existing;
    }

    // Delete Term | Must NOT have any contigent classes
    public async Task DeleteTerm(int id)
    {
        var existing = await GetTermById(id);
        // Verify that NO Class sessions remain in the term
        if (existing.ClassSessions.Count > 0) throw new Exception("Cannot delete this term as long as it has active class sessions attached to it");
        // Delete Term
        else await _context.TermDb.Where(x => x.Id == id).ExecuteDeleteAsync();
        
    }



}