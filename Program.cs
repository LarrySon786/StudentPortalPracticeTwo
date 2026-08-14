using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components;
using StudentPortalPracticeTwo.Components.Services.Application;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Students;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using StudentPortalPracticeTwo.Components.Services.EmailServices;
using StudentPortalPracticeTwo.Components.Services.Students;
using DotNetEnv;
using StudentPortalPracticeTwo.Components.Services.Authentication;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;
using System.Text.Json;
using StudentPortalPracticeTwo.Database.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authorization | authentication | Identity Core | Cookie Settings
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>() // Identity Core auth features
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options => // Cookie configurations
{
    options.Cookie.Name = "StudentPortal.Identity";
    options.Cookie.HttpOnly = true; // This is enforced by default by identity. It makes it so Javascript cannot read requests. Http only. For security and protection against certain attacks.
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.LoginPath = "/login"; // This redirects users to /login if they are not authenticated.
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Scoped Services
builder.Services.AddScoped<DraftApplicationDb>(); // Student Application
builder.Services.AddScoped<FinalApplicationDb>(); // Student Application
builder.Services.AddScoped<DegreeService>(); // Admin Service - create degrees
builder.Services.AddScoped<CourseService>(); // Admin Service - create courses/classes
builder.Services.AddScoped<ClassSessionService>(); // Admin Service - create class sessions
builder.Services.AddScoped<TermService>(); // Term Service - create and manage terms for students to register in
builder.Services.AddScoped<IEmailService, EmailService>(); // Email services | Used in approval / declined application letters
builder.Services.AddScoped<UserService>(); // Service to manage Users, reset passwords, and more
builder.Services.AddScoped<AuthLogin>(); // Login, logout, and other auth methods
builder.Services.AddScoped<TermServiceHelper>();
DotNetEnv.Env.Load();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Login Http Endpoint
app.MapPost("/account/login", async (HttpContext context, SignInManager<ApplicationUser> signInManager) =>
{
    var form = await context.Request.ReadFormAsync();

    var email = form["Email"].ToString();
    var password = form["Password"].ToString();

    if (string.IsNullOrEmpty(email)) return Results.Redirect("/login");
    if (string.IsNullOrEmpty(password)) return Results.Redirect("/login");

    var result = await signInManager.PasswordSignInAsync(email, password, true, false);

    if (result.Succeeded) return Results.Redirect("/dashboard");
    return Results.Redirect("/login?error=true");
});

// LOGOUT
app.MapPost("/account/logout", async (
    SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();

    return Results.Redirect("/login");
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// RESET DB for testing purposes. Clears all database data. Visit the route: '/rest-db'
if (app.Environment.IsDevelopment())
{
    app.MapGet("/reset-db", async (IServiceScopeFactory scopeFactory) =>
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var termService = scope.ServiceProvider.GetRequiredService<TermService>();

        var degreeService = scope.ServiceProvider.GetRequiredService<DegreeService>();

        var courseService = scope.ServiceProvider.GetRequiredService<CourseService>();

        var classSessionService = scope.ServiceProvider.GetRequiredService<ClassSessionService>();

        await signInManager.SignOutAsync();
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();

        // Create roles for authorization
        if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("Student")) await roleManager.CreateAsync(new IdentityRole("Student"));


        // Create Default Terms
        var termOne = new Term()
        {
            Season = TermSeason.Fall,
            Year = 2026,
            AvailableToRegisterClasses = true,
        };

        var termTwo = new Term()
        {
            Season = TermSeason.Spring,
            Year = 2027,
            AvailableToRegisterClasses = true,
        };
        await termService.CreateTerm(termOne);
        await termService.CreateTerm(termTwo);


       // Create Courses
        var jsonCourses = File.ReadAllText("Database/JSON/Courses/SoftwareEngineering.json"); // Course JSON
        var courseDefinition = JsonSerializer.Deserialize<List<JsonCourse>>( jsonCourses , new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true });

        foreach (JsonCourse courseItem in courseDefinition!) // Iterate through all courses to create
        {
            var course = new Course()
            {
                Name = courseItem.Name,
                Code = courseItem.Code,
                Credits = courseItem.Credits,
            };
            await courseService.CreateCourse(course); // Save
        }

        // Create Default Degree
        var jsonDegree = File.ReadAllText("Database/JSON/Degrees/SoftwareEngineering.json"); // Get JSON of degree
        var degreeDefinition = JsonSerializer.Deserialize<JsonDegree>(
            jsonDegree, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        Degree degree = new() // Create Degree
        {
            Name = "Software Engineering",
            Description = "Software Engineering is the finnest dicipline.",
            Courses = [],
        };
        foreach (string code in degreeDefinition!.Courses) // Add all courses
        {
            var course = db.CourseDb.Single(c => c.Code == code);
            degree.Courses.Add(course);
        }
        await degreeService.CreateDegree(degree); // Save


        // Create Course Sessions
        var jsonSessions = File.ReadAllText("Database/JSON/ClassSessions.Json");
        var classSessionDefinition = JsonSerializer.Deserialize<List<JsonClassSessions>>(
            jsonSessions, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        foreach (var session in classSessionDefinition!)
        {
            var course = db.CourseDb.Single(s => s.Code == session.CourseCode);
            var term = db.TermDb.Single(s => s.Season.ToString() + s.Year.ToString() == $"{session.Term}");

            var createdSession = new ClassSession()
            {
                Course = course,
                Instructor = session.Instructor,
                Location = session.Location,
                Capacity = session.Capacity,
                CurrentCount = session.CurrentCount,
                StartDate = session.StartDate,
                StartTime = session.StartTime,
                EndDate = session.EndDate,
                EndTime = session.EndTime,
                Description = session.Description,
                Term = term,

            };

            await classSessionService.CreateClassSession(createdSession);
        }

        // Create default admin role
        UserEmergencyContactModel emergencyContactNumber = new()
        {
            ContactName = "Angie",
            Phone = "330-333-3333",
            Relationship = "Mother",
        };
        var admin = new UserModel()
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "richardsbrandon4@gmail.com",
            DateOfBirth = new DateOnly(2001, 6, 26),
            ContactDetails = new()
            {
                Phone = "111-222-3333"
            },
            EmergencyContact = [emergencyContactNumber],
            MyProgram = new()
            {
                MyDegree = degree,
            },
            IdentityUserId = null!,
            FinalApplicationId = null,
            OriginalFinalApplication = null,
        };
        await userService.CreateAdmin(admin, "Brandoniscool1234$");


        return "Database reset";
    });
}



app.Run();
