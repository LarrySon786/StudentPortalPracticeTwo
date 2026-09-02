using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components;
using StudentPortalPracticeTwo.Components.Services.Application;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using StudentPortalPracticeTwo.Components.Services.EmailServices;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using DotNetEnv;
using StudentPortalPracticeTwo.Components.Services.Authentication;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Components.Services.Admin.SeedDatabase;
using StudentPortalPracticeTwo.Components.Services.SupportTicketServices;

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
builder.Services.AddDbContextFactory<ApplicationDbContext>( options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

// Scoped Services
builder.Services.AddScoped<DraftApplicationDb>(); // Student Application
builder.Services.AddScoped<FinalApplicationDb>(); // Student Application
builder.Services.AddScoped<DegreeService>(); // Admin Service - create degrees
builder.Services.AddScoped<CourseService>(); // Admin Service - create courses/classes
builder.Services.AddScoped<ClassSessionService>(); // Admin Service - create class sessions
builder.Services.AddScoped<TermService>(); // Term Service - create and manage terms for students to register in
builder.Services.AddScoped<AssignmentService>(); // Assignments service - Instructors create assignemnts for class Sessions
builder.Services.AddScoped<IEmailService, EmailService>(); // Email services | Used in approval / declined application letters
builder.Services.AddScoped<UserService>(); // Service to manage Users, reset passwords, and more
builder.Services.AddScoped<StudentService>(); // Service to manage creation of Students
builder.Services.AddScoped<FacultyService>(); // Service to manage creation of faculty members
builder.Services.AddScoped<AdminService>(); // Service to manage creation of Admin members
builder.Services.AddScoped<AuthLogin>(); // Login, logout, and other auth methods
builder.Services.AddScoped<RegisterCourses>(); // Allows students to register, drop, and manage their courses
builder.Services.AddScoped<TermManagement>(); // Allows instructors to manage class sessions for each term
builder.Services.AddScoped<SupportTicketService>(); // Allows support ticket operations between students and admins
builder.Services.AddControllers();
builder.Services.AddScoped<TermServiceHelper>();
// DATABASE SEEDING
// Database Reset / Seeding
builder.Services.AddScoped<ResetDatabase>();
builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<TermSeeder>();
builder.Services.AddScoped<CourseSeeder>();
builder.Services.AddScoped<DegreeSeeder>();
builder.Services.AddScoped<AdminSeeder>();
builder.Services.AddScoped<FacultySeeder>();
builder.Services.AddScoped<ClassSessionSeeder>();
builder.Services.AddScoped<StudentSeeder>();
builder.Services.AddScoped<ApplicationSeeder>();
builder.Services.AddScoped<DevSeeder>();

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
app.MapPost("/account/login", async (HttpContext context, SignInManager<ApplicationUser> signInManager
    , UserService userService) =>
{
    var form = await context.Request.ReadFormAsync();

    var email = form["Email"].ToString();
    var password = form["Password"].ToString();

    if (string.IsNullOrEmpty(email)) return Results.Redirect("/login");
    if (string.IsNullOrEmpty(password)) return Results.Redirect("/login");

    var user = await userService.GetUserByEmail(email);
    if (user == null || user.IsDisabled == true) return Results.Redirect("/login?error=true");

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

// Download transcripts enpoints
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
