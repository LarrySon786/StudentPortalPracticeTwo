
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class DraftApplicationDb
{
    private readonly ApplicationDbContext _context;
    private readonly DegreeService _degree;

    public DraftApplicationDb(ApplicationDbContext context, DegreeService degree)
    {
        _context = context;
        _degree = degree;
    }


    public async Task<DraftApplicationModel?> GetByEmail(string email)
    {
        var entity = await _context.DraftApplicationDb
            .Include(x => x.DraftStudentInfo)
            .Include(x => x.DraftStudentContact)
            .Include(x => x.DraftEmergencyContact)
            .Include(x => x.DraftProgramSelection)
            .Include(x => x.DraftAcademicHistory)
            .Include(x => x.DraftEssays)
            .FirstOrDefaultAsync(x => x.Email == email);

        return entity;
    }

    public async Task<DraftApplicationModel?> GetById(int id)
    {
        var entity = await _context.DraftApplicationDb
            .Include(x => x.DraftStudentInfo)
            .Include(x => x.DraftStudentContact)
            .Include(x => x.DraftEmergencyContact)
            .Include(x => x.DraftProgramSelection)
            .Include(x => x.DraftAcademicHistory)
            .Include(x => x.DraftEssays)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<DraftApplicationModel> CreateApplication(string email)
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

        _context.DraftApplicationDb.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateApplication(DraftApplicationModel updated)
    {
        DraftApplicationModel? existing = await _context.DraftApplicationDb
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

        existing.DraftProgramSelection = updated.DraftProgramSelection;

        existing.DraftAcademicHistory = updated.DraftAcademicHistory;

        existing.DraftEssays = updated.DraftEssays;
        

        await _context.SaveChangesAsync();
    }



}



