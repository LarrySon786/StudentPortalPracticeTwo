
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class DraftApplicationDb
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly DegreeService _degree;

    public DraftApplicationDb(IDbContextFactory<ApplicationDbContext> context, DegreeService degree)
    {
        _context = context;
        _degree = degree;
    }


    public async Task<DraftApplicationModel?> GetByEmail(string email, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var entity = await context.DraftApplicationDb
                .Include(x => x.DraftStudentInfo)
                .Include(x => x.DraftStudentContact)
                .Include(x => x.DraftEmergencyContact)
                .Include(x => x.DraftProgramSelection)
                .Include(x => x.DraftAcademicHistory)
                .Include(x => x.DraftEssays)
                .FirstOrDefaultAsync(x => x.Email == email);

            return entity;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task<DraftApplicationModel?> GetById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var entity = await context.DraftApplicationDb
                .Include(x => x.DraftStudentInfo)
                .Include(x => x.DraftStudentContact)
                .Include(x => x.DraftEmergencyContact)
                .Include(x => x.DraftProgramSelection)
                .Include(x => x.DraftAcademicHistory)
                .Include(x => x.DraftEssays)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task<DraftApplicationModel> CreateApplication(string email, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
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

            context.DraftApplicationDb.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task UpdateApplication(DraftApplicationModel updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            DraftApplicationModel? existing = await context.DraftApplicationDb
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

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }



}



