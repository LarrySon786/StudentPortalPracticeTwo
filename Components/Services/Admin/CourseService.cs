

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class CourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL COURSES
    public async Task<List<Course>> GetAllCourses()
    {
        return await _context.CourseDb
            .Include(x => x.Degrees)
            .Include(x => x.Sessions)
            .ToListAsync();
    }

    // GET course by Id
    public async Task<Course?> GetCourseById(int id)
    {
        return await _context.CourseDb
            .Include(x => x.Degrees)
            .Include(x => x.Sessions)
            .FirstOrDefaultAsync();
    }

    // POST Course
    public async Task CreateCourse(Course course)
    {
        _context.CourseDb.Add(course);
        await _context.SaveChangesAsync();
    }

    // PUT Course
    public async Task UpdateCourse(Course updated)
    {
        Course? existing = await GetCourseById(updated.Id);
        if (existing == null) throw new Exception("No existing course found. Updating course failed.");

        existing.Code = updated.Code;
        existing.Credits = updated.Credits;
        existing.Degrees = updated.Degrees;
        existing.Name = updated.Name;
        existing.Sessions = updated.Sessions;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCourse(int id)
    {
        _context.CourseDb
            .Where(x => x.Id == id)
            .ExecuteDelete();
    }
}