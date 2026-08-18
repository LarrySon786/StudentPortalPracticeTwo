

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class CourseService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;

    public CourseService(IDbContextFactory<ApplicationDbContext> context)
    {
        _context = context;
    }

    // GET ALL COURSES
    public async Task<List<Course>> GetAllCourses(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var result = await context.CourseDb
                .Include(x => x.Degrees)
                .Include(x => x.Sessions)
                .ToListAsync();

            return result;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // GET course by Id
    public async Task<Course?> GetCourseById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.CourseDb
                .Include(x => x.Degrees)
                .Include(x => x.Sessions)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // POST Course
    public async Task CreateCourse(Course course, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            context.CourseDb.Add(course);
            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    // PUT Course
    public async Task UpdateCourse(Course updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            Course? existing = await GetCourseById(updated.Id);
            if (existing == null) throw new Exception("No existing course found. Updating course failed.");

            existing.Code = updated.Code;
            existing.Credits = updated.Credits;
            existing.Degrees = updated.Degrees;
            existing.Name = updated.Name;
            existing.Sessions = updated.Sessions;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task DeleteCourse(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            context.CourseDb
                .Where(x => x.Id == id)
                .ExecuteDelete();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }
}