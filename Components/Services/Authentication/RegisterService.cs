

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database;

namespace StudentPortalPracticeTwo.Components.Services.Authentication;

public class RegisterService
{
    private readonly ApplicationDbContext _context;

    public RegisterService(ApplicationDbContext context)
    {
        _context = context;
    }
}