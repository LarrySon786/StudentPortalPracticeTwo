
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class DraftApplicationDb
{
    private readonly ApplicationDbContext _context;

    public DraftApplicationDb(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<DraftApplicationModel?> GetByEmail(string email)
    {
        var entity = await _context.DraftApplicationDb
            .Include(x => x.DraftStudentInfo)
            .Include(x => x.DraftStudentContact)
            .FirstOrDefaultAsync(x => x.Email == email);

        return entity;
    }

    public async Task<DraftApplicationModel?> GetById(int id)
    {
        var entity = await _context.DraftApplicationDb
            .Include(x => x.DraftStudentInfo)
            .Include(x => x.DraftStudentContact)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<DraftApplicationModel> CreateApplication(string email)
    {
        DraftApplicationModel entity = new()
        {
            Email = email,
            DraftStudentInfo = new(),
            DraftStudentContact = new()
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
            .FirstOrDefaultAsync(x => x.Id == updated.Id);

        if (existing == null)
        {
            throw new Exception("No application found");
        }

        existing.Email = updated.Email;

        existing.DraftStudentInfo?.FirstName = updated.DraftStudentInfo?.FirstName;
        existing.DraftStudentInfo?.LastName = updated.DraftStudentInfo?.LastName;

        existing.DraftStudentContact?.Phone = updated.DraftStudentContact?.Phone;

        existing.DraftEmergencyContact = updated.DraftEmergencyContact;

        await _context.SaveChangesAsync();
    }



}



