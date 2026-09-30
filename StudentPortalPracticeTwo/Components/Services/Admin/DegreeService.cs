

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class DegreeService
{
    private readonly CreateDisposeContextHelper _createDispose;

    public DegreeService(CreateDisposeContextHelper createDispose)
    {
        _createDispose = createDispose;
    }

    // GET all degrees
    public async Task<List<Degree>> GetAllDegrees(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => DegreeQuery(db).ToListAsync(), context);
    }

    // GET degree by id
    public async Task<Degree?> GetDegreeById(int? id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            if (id == null) throw new Exception("Could not get degree by id. Id received in parameter is null");
            return await DegreeQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }, context);
    }

    // POST degree created
    public async Task CreateDegree(Degree newDegree, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var errors = ValidationHelper.ValidateModel(newDegree); // Server-side validation
            if (errors.Count() > 0)
            {
                string errorMessage = string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage));
                throw new ValidationException($"Could not create degree. \n {errorMessage}");
            }

            db.DegreeDb.Add(newDegree);
            await db.SaveChangesAsync();
        }, context);
    }

    // PUT existing Degree
    public async Task UpdateDegree(Degree updated, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var errors = ValidationHelper.ValidateModel(updated); // Server Side Validation
            if (errors.Count() > 0)
            {
                string errorMessage = string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage));
                throw new ValidationException($"Could not update degree. \n {errorMessage}");
            }

            Degree? existing = await GetDegreeById(updated.Id, db);
            if (existing == null) throw new Exception("No existing degree found. Cannot update");

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Courses = updated.Courses;

            await db.SaveChangesAsync();
        }, context);
    }


    // DELETE existing Degree
    public async Task DeleteDegree(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            await db.DegreeDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            await db.SaveChangesAsync();
        }, context);
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

