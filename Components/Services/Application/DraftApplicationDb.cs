
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class DraftApplicationDb
{
    private readonly DegreeService _degree;
    private readonly CreateDisposeContextHelper _createDispose;

    public DraftApplicationDb(DegreeService degree, CreateDisposeContextHelper createDispose)
    {
        _degree = degree;
        _createDispose = createDispose;
    }


    public async Task<DraftApplicationModel?> GetByEmail(string email, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var entity = await db.DraftApplicationDb
                .Include(x => x.DraftStudentInfo)
                .Include(x => x.DraftStudentContact)
                .Include(x => x.DraftEmergencyContact)
                .Include(x => x.DraftProgramSelection)
                .Include(x => x.DraftAcademicHistory)
                .Include(x => x.DraftEssays)
                .FirstOrDefaultAsync(x => x.Email == email);

            return entity;
        }, context);
    }

    public async Task<DraftApplicationModel?> GetById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var entity = await db.DraftApplicationDb
                .Include(x => x.DraftStudentInfo)
                .Include(x => x.DraftStudentContact)
                .Include(x => x.DraftEmergencyContact)
                .Include(x => x.DraftProgramSelection)
                .Include(x => x.DraftAcademicHistory)
                .Include(x => x.DraftEssays)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }, context);
    }

    public async Task<DraftApplicationModel> CreateApplication(string email, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {

            DraftApplicationModel entity = new()
            {
                Email = email,
                DraftStudentInfo = new(),
                DraftStudentContact = new(),
                DraftEmergencyContact = new(),
                DraftProgramSelection = new(),
                DraftAcademicHistory = new(),
                DraftEssays = new(),
            };

            db.DraftApplicationDb.Add(entity);
            await db.SaveChangesAsync();

            return entity;
        }, context);
    }

    public async Task UpdateApplication(DraftApplicationModel updated, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            DraftApplicationModel? existing = await db.DraftApplicationDb
                .Include(x => x.DraftStudentInfo)
                .Include(x => x.DraftStudentContact)
                .Include(x => x.DraftEmergencyContact)
                .Include(x => x.DraftProgramSelection)
                .Include(x => x.DraftAcademicHistory)
                .Include(x => x.DraftEssays)
                .FirstOrDefaultAsync(x => x.Id == updated.Id);

            if (existing == null)
            {
                throw new Exception("No application found");
            }
        
            existing.Email = updated.Email;
            existing.DraftStudentInfo = updated.DraftStudentInfo;
            existing.DraftStudentContact = updated.DraftStudentContact;
            existing.DraftEmergencyContact = updated.DraftEmergencyContact;
            existing.DraftProgramSelection.SelectedProgramId = updated.DraftProgramSelection.SelectedProgramId;
            existing.DraftProgramSelection.StartTermId = updated.DraftProgramSelection.StartTermId;
            existing.DraftAcademicHistory = updated.DraftAcademicHistory;
            existing.DraftEssays = updated.DraftEssays;

            await db.SaveChangesAsync();
        }, context);
    }



}



