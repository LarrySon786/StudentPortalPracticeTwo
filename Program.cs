using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components;
using StudentPortalPracticeTwo.Components.Services.Application;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Users;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users.Admin;
using StudentPortalPracticeTwo.Database.Models.Users.Students;
using StudentPortalPracticeTwo.Components.Services.Admin;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using StudentPortalPracticeTwo.Components.Services.EmailServices;
using StudentPortalPracticeTwo.Components.Services.Users;
using StudentPortalPracticeTwo.Components.Services.Users.Students;
using StudentPortalPracticeTwo.Components.Services.Users.Instructors;
using DotNetEnv;
using StudentPortalPracticeTwo.Components.Services.Authentication;
using StudentPortalPracticeTwo.Components.Services.Extensions;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;
using System.Text.Json;
using StudentPortalPracticeTwo.Database.Models.DTOs;
using StudentPortalPracticeTwo.Database.Models.Application;
using System.ComponentModel;
using System.Text.Json.Serialization;

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
builder.Services.AddScoped<IEmailService, EmailService>(); // Email services | Used in approval / declined application letters
builder.Services.AddScoped<UserService>(); // Service to manage Users, reset passwords, and more
builder.Services.AddScoped<StudentService>(); // Service to manage creation of Students
builder.Services.AddScoped<FacultyService>(); // Service to manage creation of faculty members
builder.Services.AddScoped<AdminService>(); // Service to manage creation of Admin members
builder.Services.AddScoped<AuthLogin>(); // Login, logout, and other auth methods
builder.Services.AddScoped<RegisterCourses>(); // Allows students to register, drop, and manage their courses
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
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        var facultyService = scope.ServiceProvider.GetRequiredService<FacultyService>();

        var adminService = scope.ServiceProvider.GetRequiredService<AdminService>();

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
        await termService.CreateTerm(termOne, db);
        await termService.CreateTerm(termTwo, db);


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
            await courseService.CreateCourse(course, db); // Save
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
        await degreeService.CreateDegree(degree, db); // Save

        // Create Instructors
        var jsonInstructors = File.ReadAllText("Database/JSON/Users/Faculty/Faculty.Json");
        var facultyDefinition = JsonSerializer.Deserialize<List<JsonFaculty>>(
            jsonInstructors, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        foreach (JsonFaculty faculty in facultyDefinition!)
        {
            // Create faculty member's user Identity
            var identity = new ApplicationUser()
            {
                UserName = faculty.Email,
                Email = faculty.Email,
            };

            var result = await userManager.CreateAsync(identity);
            if (!result.Succeeded) throw new Exception("Could not create Instructor's user Identity in database seeding");

            List<UserEmergencyContactModel> emergencyContacts = new(); // Prepare emergency contacts
            foreach (var contact in faculty.EmergencyContact)
            {
                var newContact = new UserEmergencyContactModel()
                {
                    ContactName = contact.ContactName,
                    Relationship = contact.Relationship,
                    Phone = contact.Phone,
                };
                emergencyContacts.Add(newContact);
            }

            var entity = new Faculty() // Create each faculty member
            {
                FirstName = faculty.FirstName,
                MiddleName = faculty.MiddleName,
                LastName = faculty.LastName,
                Email = faculty.Email,
                DateOfBirth = faculty.DateOfBirth,
                ContactDetails = new()
                {
                    Phone = faculty.ContactDetails.Phone
                },
                EmergencyContact = emergencyContacts,
                IdentityUserId = identity.Id
            };
            await facultyService.CreateFaculty(entity, db);
        }



        // Create Course Sessions
        var jsonSessions = File.ReadAllText("Database/JSON/ClassSessions.Json");
        var classSessionDefinition = JsonSerializer.Deserialize<List<JsonClassSessions>>(
            jsonSessions, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        foreach (var session in classSessionDefinition!)
        {
            var course = db.CourseDb.Single(s => s.Code == session.CourseCode);
            var term = db.TermDb.Single(s => s.Season.ToString() + s.Year.ToString() == $"{session.Term}");
            var instructor = db.FacultyDb.Single(s => s.Email == session.InstructorEmail);


            var createdSession = new ClassSession()
            {
                Course = course,
                Instructor = instructor,
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

            await classSessionService.CreateClassSession(createdSession, db);
        }

        // Create default admin role
        UserEmergencyContactModel emergencyContactNumber = new()
        {
            ContactName = "Angie",
            Phone = "330-333-3333",
            Relationship = "Mother",
        };
        var admin = new AdminModel()
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
            IdentityUserId = null!,
        };
        await adminService.CreateAdmin(admin, "Brandoniscool1234$", db);

        // Create Submited Application
        var file = File.ReadAllText("Database/JSON/Applications/FinalApplication.json");
        try
        {
            var applicationDefinition = JsonSerializer.Deserialize<List<JsonFinalApplication>>(file, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });


        
        foreach ( var student in applicationDefinition!)
        {
            // Get existing Degree and Term from database
            var identifiedDegree = await degreeService.GetDegreeById(student.StudentProgram.DegreeId, db);
            var identifiedTerm = await termService.GetTermById(student.StudentProgram.TermId, db);
            

            ApplicationModel newApplication = new()
            {
                Email = student.Email,
                ApprovedStatus = student.ApprovedStatus,

                // Student Information
                StudentInfo = new StudentInfoModel()
                {
                    FirstName = student.StudentInfo.FirstName,
                    MiddleName = student.StudentInfo.MiddleName,
                    LastName = student.StudentInfo.LastName,
                    DateOfBirth = student.StudentInfo.DateOfBirth,
                    Race = student.StudentInfo.Race,
                    Gender = student.StudentInfo.Gender,
                    CitizenshipCountry = student.StudentInfo.CitizenshipCountry,
                    StreetOneAddress = student.StudentInfo.StreetOneAddress,
                    StreetTwoAddress = student.StudentInfo.StreetTwoAddress,
                    City = student.StudentInfo.City,
                    StateOrProvince = student.StudentInfo.StateOrProvince,
                    Zipcode = student.StudentInfo.Zipcode
                },

                // Student Contact
                StudentContact = new StudentContactModel()
                {
                    Phone = student.StudentContact.Phone,
                    AltPhone = student.StudentContact.AltPhone
                },

                // Emergency Contacts
                EmergencyContact = student.EmergencyContacts
                    .Select(contact => new EmergencyContactModel()
                    {
                        ContactName = contact.ContactName,
                        Relationship = contact.Relationship,
                        Phone = contact.Phone
                    })
                    .ToList(),

                // Student Program
                StudentProgram = new StudentProgram()
                {
                    SelectedProgram = identifiedDegree!,
                    StartTerm = identifiedTerm
                },

                // Academic History
                AcademicHistory = new AcademicHistoryModel()
                {
                    HighschoolTranscriptFileName = student.AcademicHistory.HighschoolTranscriptFileName,

                    HighschoolTranscript = Convert.FromBase64String(student.AcademicHistory.HighschoolTranscript),

                    CollegeTranscriptFileName = student.AcademicHistory.CollegeTranscriptFileName,

                    CollegeTranscript = student.AcademicHistory.CollegeTranscript == null
                            ? null
                            : Convert.FromBase64String( student.AcademicHistory.CollegeTranscript )
                },

                // Essays
                Essays = new StudentEssayModel()
                {
                    ResponseOne = student.Essays.ResponseOne,
                    ResponseTwo = student.Essays.ResponseTwo,
                    ResponseThree = student.Essays.ResponseThree
                }
            };
            db.ApplicationDb.Add(newApplication);
        }}
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON ERROR: {ex.Message}");
            Console.WriteLine($"PATH: {ex.Path}");
            Console.WriteLine($"LINE: {ex.LineNumber}");
            Console.WriteLine($"POSITION: {ex.BytePositionInLine}");

            throw;
        }
        await db.SaveChangesAsync(); // Save added final Applications
        return "Database reset";
    });
}



app.Run();
