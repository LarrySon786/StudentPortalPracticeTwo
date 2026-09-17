
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Authentication;
using Superpower.Model;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class DraftApplicationDb
{
    private readonly DegreeService _degree;
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public DraftApplicationDb(DegreeService degree, CreateDisposeContextHelper createDispose, IEmailService emailService,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        _degree = degree;
        _createDispose = createDispose;
        _emailService = emailService;
        _configuration = configuration;
        _environment = environment;
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

    // Send Verification code
    public async Task SendApplicantVerificationEmail(string email, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetByEmail(email, context);
            if (existing == null) throw new Exception("No application found with this email.");

            // Form Email and Code
            string subject = "Verify Identity | CSU Application";
            var htmlTemplatePath = Path.Combine(_environment.ContentRootPath, "Components", "Ui", "EmailTemplates", "VerifyEmail.html"); // Approved Email Template
            var verificationCode = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var html = await File.ReadAllTextAsync(htmlTemplatePath); //Template 
            html = html.Replace("{{verification_code}}", verificationCode);

            // HASH the verification code
            var hashedCode = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(verificationCode))
            );

            // Store verification code in database
            db.DraftVerificationCode.Add(new ApplicationVerificationCode()
            {
                HashedCode = hashedCode,
                DraftApplicationId = existing.Id,
            });
            await db.SaveChangesAsync();

            // Send Invite via Email
            await _emailService.SendEmailAsync(email, "Applicant", subject, html);

        }, context);
    }

    // Confirm Applicant Identity by Code || THIS IS DONE in program.cs under the routes that are mapped.
    public async Task<bool> VerifyCodeSentInEmail(string email, string code, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            // Obtain and hash user form's code
            var hashedInput = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(code))
            );

            // Obtain stored code from database
            var result = await db.DraftVerificationCode
                .Include(x => x.DraftApplication)
                .FirstOrDefaultAsync(x => x.DraftApplication!.Email == email &&
                    x.ExpirationTime > DateTime.UtcNow &&
                    x.Used == false);
            if (result == null) throw new Exception("No results found in database. ");

            // Compare entered code with database stored code
            if (result.HashedCode == hashedInput)
            {
                result.Used = true;
                await db.SaveChangesAsync();
                return true;
            }
            // If they do not match:
            else
            {
                result.FailedAttempts++;
                await db.SaveChangesAsync();
                return false;
            }
        }, context);
    }

    

}



