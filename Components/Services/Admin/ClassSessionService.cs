
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Admin;

public class ClassSessionService
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly CreateDisposeContextHelper _createDispose;

    public ClassSessionService(IDbContextFactory<ApplicationDbContext> context, CreateDisposeContextHelper createDispose)
    {
        _context = context;
        _createDispose = createDispose;
    }

    // GET all class sessions (order by future to past)
    public async Task<List<ClassSession>> GetAllClassSessions(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => ClassSessionQuery(db)
                .ToListAsync()
            , context);
    }

    // GET class session by Id
    public async Task<ClassSession?> GetClassSessionById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db => await ClassSessionQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync()
            , context);
    }

    // POST class session
    public async Task<ClassSession> CreateClassSession(ClassSession session, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            db.ClassSessionDb.Add(session);
            await db.SaveChangesAsync();
            return session;
        }, context);
    }

    // PUT class session
    public async Task UpdateClassSession(ClassSession updated, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync( async db => {
            ClassSession? existing = await GetClassSessionById(updated.Id, db);
            if (existing == null) throw new Exception("No existing class session was found. Update failed");

            existing.InstructorId = updated.InstructorId;
            existing.Capacity = updated.Capacity;
            existing.CurrentCount = updated.CurrentCount;
            existing.Description = updated.Description;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            existing.StartTime = updated.StartTime;
            existing.EndTime = updated.EndTime;
            existing.Location = updated.Location;
            existing.TermId = updated.TermId;
            existing.CourseId = updated.CourseId;

            await db.SaveChangesAsync();
        }, context);
    }

    // DELETE existing class session
    public async Task DeleteClassSession(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(db => db.ClassSessionDb
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(), context);
    }

    // Archive class session | Used to save records of completed class sessions
    public async Task ArchiveAndCloseClassSession(int classSessionId, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync( async db => {
            // Obtain class session
            var existing = await GetClassSessionById(classSessionId, db);
            if (existing == null) throw new Exception("Could not find an existing class session to archive");

            // Make sure no students are still enrolled in this class
            foreach (UserProgramModel program in existing.StudentProgramModels)
            {
                if (program.CurrentSessions.Any(x => x == existing)) throw new Exception("Could not archive class session. Some students are still enrolled in this session");
            }

            existing.ArchivedAndClosed = true;
            await db.SaveChangesAsync();
        }, context);
            
    }

    // Unarchive class session | Used to save records of completed class sessions
    public async Task UnArchiveAndOpenClassSession(int classSessionId, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // Obtain class session
            var existing = await GetClassSessionById(classSessionId, db);
            if (existing == null) throw new Exception("Could not find an existing class session to archive");

            existing.ArchivedAndClosed = false;
            await db.SaveChangesAsync();
        }, context);
            
        
    }

    // QUERY for comprehensive GET requests
    private IQueryable<ClassSession> ClassSessionQuery(ApplicationDbContext context)
    {
        return context.ClassSessionDb
            .Include(x => x.Course)
            .Include(x => x.StudentProgramModels)
                .ThenInclude(x => x.User)
            .Include(x => x.Term)
            .Include(x => x.RegisteredStudentProgramModels)
                .ThenInclude(x => x.User)
            .Include(x => x.Assignments)
                .ThenInclude(x => x.Grades)
                    .ThenInclude(x => x.StudentProgram)
                        .ThenInclude(x => x.User)
            .Include(x => x.Graduates)
                .ThenInclude(x => x.StudentProgram)
                    .ThenInclude(x => x.User)
            .Include(x => x.FailedCourses)
                .ThenInclude(x => x.StudentProgram)
                    .ThenInclude(x => x.User)
            .Include(x => x.Graduates)
                .ThenInclude(x => x.Course)
            .Include(x => x.FailedCourses)
                .ThenInclude(x => x.Course);

    }


}