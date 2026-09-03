

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class CourseService
{
    private readonly CreateDisposeContextHelper _createDispose;

    public CourseService(CreateDisposeContextHelper createDispose)
    {
        _createDispose = createDispose;
    }

    // GET ALL COURSES
    public async Task<List<Course>> GetAllCourses(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var result = await db.CourseDb
                .Include(x => x.Degrees)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.Term)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.RegisteredStudentProgramModels)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.StudentProgramModels)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.Instructor)
                .ToListAsync();

            return result;
        }, context);
    }

    // GET course by Id
    public async Task<Course?> GetCourseById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => db.CourseDb
                .Include(x => x.Degrees)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.Term)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.RegisteredStudentProgramModels)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.StudentProgramModels)
                .Include(x => x.Sessions)
                    .ThenInclude(x => x.Instructor)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(), context);
    }

    // POST Course
    public async Task CreateCourse(Course course, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            db.CourseDb.Add(course);
            await db.SaveChangesAsync();
        }, context);
    }

    // PUT Course
    public async Task UpdateCourse(Course updated, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            Course? existing = await GetCourseById(updated.Id, db);
            if (existing == null) throw new Exception("No existing course found. Updating course failed.");

            existing.Code = updated.Code;
            existing.Credits = updated.Credits;
            existing.Degrees = updated.Degrees;
            existing.Name = updated.Name;
            existing.Sessions = updated.Sessions;

            await db.SaveChangesAsync();
        }, context);
    }

    public async Task DeleteCourse(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(db => db.CourseDb
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync(), context);
    }
}