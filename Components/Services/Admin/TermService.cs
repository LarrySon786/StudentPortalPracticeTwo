

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class TermService
{
    private readonly CreateDisposeContextHelper _createDispose;

    public TermService(CreateDisposeContextHelper createDispose)
    {
        _createDispose = createDispose;
    }


    // GET all terms
    public async Task<List<Term>> GetAllTerms(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await db.TermDb.ToListAsync();
            return existing;
        }, context);
    }

    // Get Term by Id
    public async Task<Term> GetTermById(int? id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            if (id == null) throw new Exception("Could not get term by Id. Id is null");
            var existing = await db.TermDb
                .Where(x => x.Id == id)
                .Include(x => x.ClassSessions)
                .FirstOrDefaultAsync();
            if (existing != null) return existing;
            else throw new Exception("No existing term was found.");
        }, context);
    }

    // Create Term
    public async Task<Term> CreateTerm(Term term, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            db.Add(term);
            await db.SaveChangesAsync();
            return term;
        }, context);
    }

    // Update Term
    public async Task<Term> UpdateTerm(Term updated, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTermById(updated.Id, db);
            if (existing == null) throw new Exception("No term found with this Id. Could not update");
            existing.Season = updated.Season;
            existing.Year = updated.Year;
            existing.ClassSessions = updated.ClassSessions;
            existing.AvailableToRegisterClasses = updated.AvailableToRegisterClasses;

            await db.SaveChangesAsync();
            return existing;
        }, context);
    }

    // Delete Term | Must NOT have any contigent classes
    public async Task DeleteTerm(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTermById(id, db);
            // Verify that NO Class sessions remain in the term
            if (existing.ClassSessions.Count > 0) throw new Exception("Cannot delete this term as long as it has active class sessions attached to it");
            // Delete Term
            else await db.TermDb.Where(x => x.Id == id).ExecuteDeleteAsync();
        }, context);

    }

}