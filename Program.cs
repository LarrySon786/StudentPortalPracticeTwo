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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Scoped Services
builder.Services.AddScoped<DraftApplicationDb>(); // Student Application
builder.Services.AddScoped<FinalApplicationDb>(); // Student Application
builder.Services.AddScoped<DegreeService>(); // Admin Service - create degrees
builder.Services.AddScoped<CourseService>(); // Admin Service - create courses/classes
builder.Services.AddScoped<ClassSessionService>(); // Admin Service - create class sessions
builder.Services.AddScoped<IEmailService, EmailService>(); // Email services | Used in approval / declined application letters
builder.Services.AddScoped<UserService>(); // Service to manage Users
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
DotNetEnv.Env.Load();


// Allow authorization cookie request to be created
builder.Services.AddHttpClient();

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


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// RESET DB for testing purposes. Clears all database data. Visit the route: '/rest-db'
if (app.Environment.IsDevelopment())
{
    app.MapGet("/reset-db", async (ApplicationDbContext db) =>
    {
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();

        return "Database reset";
    });
}

app.Run();
