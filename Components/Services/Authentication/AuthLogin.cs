
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Students;
using StudentPortalPracticeTwo.Database.Models.Students;

namespace StudentPortalPracticeTwo.Components.Services.Authentication;

public class AuthLogin
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserService _userService;

    public AuthLogin(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
                    UserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
    }

    // LOGIN USER
    public async Task LoginUser(string email, string password)
    {
        // Verify email exists
        var user = await _userService.GetUserByEmail(email);
        if (user == null) throw new Exception("No user found that matches this email.");

        // If account is disabled
        if (user.IsDisabled) throw new Exception("This account is disabled. Please contact admissions for support.");

        // Verify password was correct
        var response = await _signInManager.PasswordSignInAsync(email, password, false, false); // SignIn
        if (!response.Succeeded) throw new Exception("Invalid password.");
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



    // ***********
    // RESET AND SET PASSWORD can be found under UserServices()
    // ***********
}