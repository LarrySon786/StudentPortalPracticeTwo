using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;

namespace StudentPortalPracticeTwo.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("final-application/transcripts")]
public class FinalApplicationController : ControllerBase
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public FinalApplicationController(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    [HttpGet("highschool/{id:int}")]
    public async Task<IActionResult> DownloadHighschoolTranscript(int id)
    {

        var context = await _context.CreateDbContextAsync();

        try
        {
            var academicHistory = await context.AcademicHistoryDb.FindAsync(id);

            if (academicHistory == null || academicHistory.HighschoolTranscript == null)
                return NotFound();

            if (String.IsNullOrWhiteSpace(academicHistory.HighschoolTranscriptFileName))
                academicHistory.HighschoolTranscriptFileName = "StudentTranscript";

            return File(
                academicHistory.HighschoolTranscript,
                "application/pdf",
                academicHistory.HighschoolTranscriptFileName);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [HttpGet("college/{id:int}")]
    public async Task<IActionResult> DownloadCollegeTranscript(int id)
    {
        var context = await _context.CreateDbContextAsync();

        try
        {
            var academicHistory = await context.AcademicHistoryDb.FindAsync(id);

            if (academicHistory == null || academicHistory.CollegeTranscript == null)
                return NotFound();

            if (String.IsNullOrWhiteSpace(academicHistory.CollegeTranscriptFileName))
                academicHistory.CollegeTranscriptFileName = "StudentTranscript";
            

            return File(
                academicHistory.CollegeTranscript,
                "application/pdf",
                academicHistory.CollegeTranscriptFileName);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}