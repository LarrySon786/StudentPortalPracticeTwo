using Microsoft.EntityFrameworkCore;

using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Enums;
using StudentPortalPracticeTwo.Database.Models.SupportTicket;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.SupportTicketServices;

public class SupportTicketService
{
    private readonly CreateDisposeContextHelper _createDispose;

    public SupportTicketService(CreateDisposeContextHelper createDispose)
    {
        _createDispose = createDispose;
    }


    // GET all
    public async Task<List<SupportTicket>> GetAllTickets(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => SupportTicketQuery(db)
            .ToListAsync(), context);
    }

    // GET all by Student
    public async Task<List<SupportTicket>> GetTicketByStudentId(int studentId, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => SupportTicketQuery(db)
                .Where(x => x.StudentId == studentId)
                .ToListAsync(), context);
    }

    // Get one by Id
    public async Task<SupportTicket?> GetTicketById(int id, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(db => SupportTicketQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(), context);
    }

    // Create Support Ticket | By DTO
    public async Task<SupportTicket?> CreateNewSupportTicket(SupportTicketDto ticket, ResponseTicket response, int userId, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            // Assign the student ID
            ticket.StudentId = userId;

            // Make sure the collection exists
            ticket.ResponseTicket ??= new List<ResponseTicket>();

            // Add the response if it isn't already in the collection
            if (!ticket.ResponseTicket.Contains(response))
            {
                response.UserId = userId;   // Assign the user ID to the response
                ticket.ResponseTicket.Add(response);
            }

            // Ensure every response has the correct UserId
            foreach (var responseTicket in ticket.ResponseTicket)
            {
                responseTicket.UserId = userId;
            }

            // Create the SupportTicket entity
            var entity = new SupportTicket
            {
                Title = ticket.Title,
                Status = SupportStatus.Submitted,
                Topic = ticket.Topic,
                StudentId = ticket.StudentId,
                ResponseTicket = ticket.ResponseTicket
            };

            db.SupportTicketsDb.Add(entity);

            await db.SaveChangesAsync();

            return entity;
        }, context);
    }

    // Add Response to existing support ticket
    public async Task<SupportTicket?> AddResponseToTicketById(int id, ResponseTicket response, int userId, ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTicketById(id, db);
            if (existing == null) throw new Exception("No existing ticket found");

            response.UserId = userId;
            existing.ResponseTicket.Add(response); // Add response to message chain

            await db.SaveChangesAsync();
            return existing;
        }, context);
    }




    // ********************** | Tools to mark a support ticket status
    // Mark as Pending
    public async Task MarkPending(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTicketById(id, db);
            if (existing == null) throw new Exception("No existing ticket found with this Id.");

            existing.Status = SupportStatus.Pending;

            await db.SaveChangesAsync();
        }, context);
    }

    // Mark as Submitted
    public async Task MarkSubmitted(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTicketById(id, db);
            if (existing == null) throw new Exception("No existing ticket found with this Id.");

            existing.Status = SupportStatus.Submitted;

            await db.SaveChangesAsync();
        }, context);
    }

    // Mark as Resolved
    public async Task MarkResolved(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTicketById(id, db);
            if (existing == null) throw new Exception("No existing ticket found with this Id.");

            existing.Status = SupportStatus.Resolved;

            await db.SaveChangesAsync();
        }, context);
    }

    // Mark as Awaiting
    public async Task MarkAwaiting(int id, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            var existing = await GetTicketById(id, db);
            if (existing == null) throw new Exception("No existing ticket found with this Id.");

            existing.Status = SupportStatus.Awaiting;

            await db.SaveChangesAsync();
        }, context);
    }

    // **********************

    // General Query
    private IQueryable<SupportTicket> SupportTicketQuery(ApplicationDbContext context)
    {
        return context.SupportTicketsDb
            .Include(x => x.Student)
            .Include(x => x.ResponseTicket)
                .ThenInclude(x => x.User);
    }




}