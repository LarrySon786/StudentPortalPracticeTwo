
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Components.Services.Authentication;

public class AuthLogin
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserService _userService;
    private readonly CreateDisposeContextHelper _createDispose;
    private readonly IHttpContextAccessor _httpAccessor;

    public AuthLogin(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                    UserService userService, CreateDisposeContextHelper createDispose, IHttpContextAccessor httpAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
        _createDispose = createDispose;
        _httpAccessor = httpAccessor;
    }

    // LOGIN USER
    public async Task LoginUser(string email, string password, ApplicationDbContext? context = null)
    {
        await _createDispose.ExecuteAsync(async db =>
        {
            // Verify email exists
            var user = await _userService.GetUserByEmail(email, db);
            if (user == null) throw new Exception("No user found that matches this email.");

            // If account is disabled
            if (user.IsDisabled) throw new Exception("This account is disabled. Please contact admissions for support.");

            // Verify password was correct
            var response = await _signInManager.PasswordSignInAsync(email, password, false, false); // SignIn
            if (!response.Succeeded) throw new Exception("Invalid password.");
        }, context);
    }

    // Logout User
    public async Task LogoutUser()
    {
        await _signInManager.SignOutAsync();
    }

    // GET current user 
    public async Task GetCurrentUser(ClaimsPrincipal claim)
    {
        await _userManager.GetUserAsync(claim);
    }

    // GET user for each page
    public async Task<UserModel?> GetCurrentUserAsync(ApplicationDbContext? context = null)
    {
        return await _createDispose.ExecuteAsync(async db =>
        {
            if (_httpAccessor.HttpContext == null) throw new Exception("No Http Context found.");
            var identity = _httpAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); // Primary Key is the ID

            if (identity == null) return null;

            return await db.UserDb
                .Include(x => x.ContactDetails)
                .Include(x => x.EmergencyContact)
                .Include(x => x.IdentityUser)
                .FirstOrDefaultAsync(x => x.IdentityUserId == identity);
        }, context);
    }



    // ***********
    // RESET AND SET PASSWORD can be found under UserServices()
    // ***********
}