using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Students;
using StudentPortalPracticeTwo.Components.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using StudentPortalPracticeTwo.Components.Services.Extensions;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class FinalApplicationDb
{
    private readonly IDbContextFactory<ApplicationDbContext> _context;
    private readonly UserService _userService;
    private readonly IEmailService _emailService; // 
    private readonly IWebHostEnvironment _environment; // Allows tracing back to root of project
    private readonly IConfiguration _configuration; // Gets base URL for the project 
    private readonly UserManager<ApplicationUser> _userManager;

    public FinalApplicationDb(IDbContextFactory<ApplicationDbContext> context, UserService userService,
        IEmailService emailService, IWebHostEnvironment environment, IConfiguration configuration,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userService = userService;
        _emailService = emailService;
        _environment = environment;
        _configuration = configuration;
        _userManager = userManager;
    }


    // GET all applications (includes denied, approved, or pending)
    public async Task<List<ApplicationModel>> GetAllApplications(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            return await context.ApplicationDb
                .Include(x => x.StudentInfo)
                .Include(y => y.StudentContact)
                .Include(x => x.EmergencyContact)
                .Include(x => x.StudentProgram)
                .Include(x => x.AcademicHistory)
                .Include(x => x.Essays)
                .ToListAsync();
        }  
         finally
        {
            if (dispose) await context.DisposeAsync();
        } 
    }

    // GET all PENDING applciations (applications that need reviewed and then approved by admins)
    public async Task<List<ApplicationModel>> GetAllPendingApplications(ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            return await context.ApplicationDb
                .Where(x => x.ApprovedStatus == Status.Pending)
                .Include(x => x.StudentInfo)
                .Include(y => y.StudentContact)
                .Include(x => x.EmergencyContact)
                .Include(x => x.StudentProgram)
                .Include(x => x.AcademicHistory)
                .Include(x => x.Essays)
                .ToListAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task<ApplicationModel?> GetByEmail(string email, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var entity = await context.ApplicationDb
                .Include(x => x.StudentInfo)
                .Include(x => x.StudentContact)
                .Include(x => x.EmergencyContact)
                .Include(x => x.StudentProgram)
                .Include(x => x.AcademicHistory)
                .Include(x => x.Essays)
                .FirstOrDefaultAsync(x => x.Email == email);

            return entity;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task<ApplicationModel?> GetById(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            var entity = await context.ApplicationDb
                .Include(x => x.StudentInfo)
                .Include(x => x.StudentContact)
                .Include(x => x.EmergencyContact)
                .Include(x => x.StudentProgram)
                .Include(x => x.StudentProgram.SelectedProgram)
                .Include(x => x.StudentProgram.StartTerm)
                .Include(x => x.AcademicHistory)
                .Include(x => x.Essays)
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task<ApplicationModel> CreateApplication(DraftApplicationModel draft, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            // VALIDATE the Draft ApplicationModel Required fields first. Identify if any fields are NULL
            // To display validation errors, please use a try{} catch{} statement to catch and update UI.
            List<string> errors = new();

            ValidateRequiredFields(draft, errors);

            // SERVER SIDE VALIDATION
            //Declare a list to hold Validation errors 
            var validationResults = new List<ValidationResult>();
            ServerSideValidation(draft, validationResults);

            // Determine if email already exists on a user.
            var existing = await GetByEmail(draft.Email, context);
            if (existing != null) errors.Add("A student already has this email. Please use a different email");

            // Add server-side errors to UI errors
            errors.AddRange(validationResults
                .Where(v => !string.IsNullOrWhiteSpace(v.ErrorMessage))
                .Select(v => v.ErrorMessage!)
            );

            // BLOCK SAVE AND THROW ERRORS IF ANY
            if (errors.Any()) throw new ValidationException(string.Join(Environment.NewLine, errors));

            // Create Emergency Contacts for final application
            var EmergencyContactList = new List<EmergencyContactModel>();
            foreach (DraftEmergencyContactModel contact in draft.DraftEmergencyContact!)
            {
                EmergencyContactModel EmergencyContact = new()
                {
                    ContactName = contact.ContactName,
                    Relationship = contact.Relationship,
                    Phone = contact.Phone,
                };
                EmergencyContactList.Add(EmergencyContact);
            }
            // Create start Term
            var startTerm = await context.TermDb.SingleAsync(x => x.Id == draft.DraftProgramSelection.StartTerm!.Id);
            // Create selected Program
            var selectedProgram = await context.DegreeDb.SingleAsync(x => x.Id == draft.DraftProgramSelection.SelectedProgram!.Id);

            // CREATE FINAL DRAFT FROM existing draft
            ApplicationModel entity = new()
            {
                Email = draft.Email,
                StudentInfo = new StudentInfoModel()
                {
                    FirstName = draft.DraftStudentInfo!.FirstName!,
                    MiddleName = draft.DraftStudentInfo!.MiddleName!,
                    LastName = draft.DraftStudentInfo!.LastName!,
                    DateOfBirth = draft.DraftStudentInfo.DateOfBirth,
                    Race = draft.DraftStudentInfo.Race!.Value,
                    Gender = draft.DraftStudentInfo.Gender!.Value,
                    CitizenshipCountry = draft.DraftStudentInfo.CitizenshipCountry!.Value,
                    StreetOneAddress = draft.DraftStudentInfo.StreetOneAddress!,
                    StreetTwoAddress = draft.DraftStudentInfo.StreetTwoAddress ?? string.Empty,
                    City = draft.DraftStudentInfo.City!,
                    StateOrProvince = draft.DraftStudentInfo.StateOrProvince!,
                    Zipcode = draft.DraftStudentInfo.Zipcode!.Value,
                },
                StudentContact = new StudentContactModel()
                {
                    Phone = draft.DraftStudentContact!.Phone!,
                    AltPhone = draft.DraftStudentContact.AltPhone,
                },
                EmergencyContact = EmergencyContactList, // Emergency Contact List created above
                StudentProgram = new StudentProgram()
                {
                    SelectedProgram = selectedProgram,
                    StartTerm = startTerm
                },
                AcademicHistory = new AcademicHistoryModel()
                {
                    HighschoolTranscriptFileName = draft.DraftAcademicHistory.HighschoolTranscriptFileName,
                    HighschoolTranscript = draft.DraftAcademicHistory.HighschoolTranscript!,
                    CollegeTranscriptFileName = draft.DraftAcademicHistory.CollegeTranscriptFileName,
                    CollegeTranscript = draft.DraftAcademicHistory.CollegeTranscript
                },
                Essays = new StudentEssayModel()
                {
                    ResponseOne = draft.DraftEssays.ResponseOne,
                    ResponseTwo = draft.DraftEssays.ResponseTwo,
                    ResponseThree = draft.DraftEssays.ResponseThree
                },

                ApprovedStatus = Status.Pending
            };
            // Validate Final Draft
            ValidateFinalDraft(entity, validationResults);

            // Add server-side errors to UI errors
            errors.AddRange(validationResults
                .Where(v => !string.IsNullOrWhiteSpace(v.ErrorMessage))
                .Select(v => v.ErrorMessage!)
            );

            // BLOCK SAVE AND THROW ERRORS IF ANY
            if (errors.Any()) throw new ValidationException(string.Join(Environment.NewLine, errors));

            context.ApplicationDb.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task UpdateApplication(ApplicationModel updated, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            ApplicationModel? entity = await GetById(updated.Id, context);

            if (entity == null)
                throw new Exception("No application found");

            entity.Email = updated.Email;
            entity.StudentInfo.FirstName = updated.StudentInfo.FirstName;
            entity.StudentInfo.MiddleName = updated.StudentInfo.MiddleName;
            entity.StudentInfo.LastName = updated.StudentInfo.LastName;
            entity.StudentInfo.Race = updated.StudentInfo.Race;
            entity.StudentInfo.Gender = updated.StudentInfo.Gender;
            entity.StudentInfo.CitizenshipCountry = updated.StudentInfo.CitizenshipCountry;
            entity.StudentInfo.StreetOneAddress = updated.StudentInfo.StreetOneAddress;
            entity.StudentInfo.StreetTwoAddress = updated.StudentInfo.StreetTwoAddress;
            entity.StudentInfo.City = updated.StudentInfo.City;
            entity.StudentInfo.StateOrProvince = updated.StudentInfo.StateOrProvince;
            entity.StudentInfo.Zipcode = updated.StudentInfo.Zipcode;

            entity.StudentContact.Phone = updated.StudentContact.Phone;
            entity.StudentContact.AltPhone = updated.StudentContact.AltPhone;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public bool ValidateRequiredFields(DraftApplicationModel draft, List<string>? errors)
    {
        if (errors == null)
            errors = new();

        // Application Information
        if (string.IsNullOrWhiteSpace(draft.Email))
            errors.Add("Please add an Email to your application.");

        // Student Information
        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.FirstName))
            errors.Add("Please add a First Name to your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.LastName))
            errors.Add("Please add a Last Name to your application.");

        if (draft.DraftStudentInfo?.Race == null)
            errors.Add("Please select a Race for your application.");

        if (draft.DraftStudentInfo?.Gender == null)
            errors.Add("Please select a Gender for your application.");

        if (draft.DraftStudentInfo?.CitizenshipCountry == null)
            errors.Add("Please select a Citizenship Country for your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.StreetOneAddress))
            errors.Add("Please add a Street Address to your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.City))
            errors.Add("Please add a City to your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.StateOrProvince))
            errors.Add("Please add a State or Province to your application.");

        if (draft.DraftStudentInfo?.Zipcode == null || draft.DraftStudentInfo.Zipcode == 0)
            errors.Add("Please add a Zip Code to your application.");

        // Contact Information
        if (string.IsNullOrWhiteSpace(draft.DraftStudentContact?.Phone))
            errors.Add("Please add a Phone Number to your application.");

        // Emergency Contact Information
        foreach (DraftEmergencyContactModel contact in draft.DraftEmergencyContact)
        {
            if (string.IsNullOrWhiteSpace(contact.ContactName))
                errors.Add("Please add a name to all your emergency contacts.");

            if (string.IsNullOrWhiteSpace(contact.Phone))
                errors.Add("Please add a Phone number to all your emergency contacts.");

            if (string.IsNullOrWhiteSpace(contact.Relationship))
                errors.Add("Please add a relationship to all of your emergency contacts.");
        }

        // Program Selection
        if (string.IsNullOrWhiteSpace(draft.DraftProgramSelection.SelectedProgram?.Name))
            errors.Add("You must select a program to apply.");

        if (string.IsNullOrWhiteSpace(draft.DraftProgramSelection.StartTerm?.DisplayName))
            errors.Add("You must select a term to apply.");

        if (draft.DraftAcademicHistory.HighschoolTranscript == null || draft.DraftAcademicHistory.HighschoolTranscript.Length == 0)
            errors.Add("You must upload a highschool transcript to apply.");

        // Highschool Transcript
        if (string.IsNullOrWhiteSpace(draft.DraftAcademicHistory.HighschoolTranscriptFileName))
        {
            errors.Add("You must have a highschool transcript uploaded to submit this application.");
        }

        // Essay Questions
        if (string.IsNullOrWhiteSpace(draft.DraftEssays.ResponseOne))
        {
            errors.Add("Essay response one is missing.");
        }
        if (string.IsNullOrWhiteSpace(draft.DraftEssays.ResponseTwo))
        {
            errors.Add("Essay response two is missing.");
        }
        if (string.IsNullOrWhiteSpace(draft.DraftEssays.ResponseThree))
        {
            errors.Add("Essay response three is missing.");
        }

        if (errors.Count == 0)
            return true;
        return false;
    }

    public bool ServerSideValidation(DraftApplicationModel draft, List<ValidationResult>? validationResults)
    {
        if (validationResults == null)
            validationResults = new();

        bool isValid = true;

        // // Validate parent
        // var parentContext = new ValidationContext(draft);
        // isValid &= Validator.TryValidateObject(
        //     draft,
        //     parentContext,
        //     validationResults,
        //     validateAllProperties: true
        // );

        // // Validate StudentInfo
        // if (draft.DraftStudentInfo != null)
        // {
        //     var studentInfoContext = new ValidationContext(draft.DraftStudentInfo);
        //     isValid &= Validator.TryValidateObject(
        //         draft.DraftStudentInfo,
        //         studentInfoContext,
        //         validationResults,
        //         validateAllProperties: true
        //     );
        // }

        // Console.WriteLine($"Phone: '{draft.DraftStudentContact.Phone}'");
        // Console.WriteLine($"AltPhone: '{draft.DraftStudentContact.AltPhone}'");
        // Console.WriteLine($"AltPhone null: {draft.DraftStudentContact.AltPhone == null}");
        // Console.WriteLine($"AltPhone length: {draft.DraftStudentContact.AltPhone?.Length}");

        // // Validate StudentContact
        // if (draft.DraftStudentContact != null)
        // {
        //     var studentContactContext = new ValidationContext(draft.DraftStudentContact);
        //     isValid &= Validator.TryValidateObject(
        //         draft.DraftStudentContact,
        //         studentContactContext,
        //         validationResults,
        //         validateAllProperties: true
        //     );
        // }

        // if (draft.DraftEssays != null)
        // {
        //     var studentEssayContext = new ValidationContext(draft.DraftEssays);
        //     isValid &= Validator.TryValidateObject(
        //         draft.DraftEssays,
        //         studentEssayContext,
        //         validationResults,
        //         validateAllProperties: true
        //     );
        // }

        return isValid;
    }

    public bool ValidateFinalDraft(ApplicationModel final, List<ValidationResult>? validationResults)
    {
        if (validationResults == null)
            validationResults = new List<ValidationResult>();

        bool isValid = true;

        if (final.StudentInfo != null)
        {
            var studentInfoContext = new ValidationContext(final.StudentInfo);
            isValid &= Validator.TryValidateObject(
                final.StudentInfo,
                studentInfoContext,
                validationResults,
                validateAllProperties: true
            );
        }

        var minimumDate = DateOnly.FromDateTime(DateTime.Today).AddYears(-12); // Minimum Age is 12 to apply
        if ((final.StudentInfo!.DateOfBirth < minimumDate) == false)
        {
            isValid = false;
            var dateResult = new ValidationResult("The minimum age to apply is 12 years old. Please change your date of birth.");
            validationResults.Add(dateResult);
        }
        

        if (final.StudentContact != null)
        {
            var studentContactContext = new ValidationContext(final.StudentContact);
            isValid &= Validator.TryValidateObject(
                final.StudentContact,
                studentContactContext,
                validationResults,
                validateAllProperties: true
            );
        }
        if (final.EmergencyContact != null)
        {
            var studentEmergencyContext = new ValidationContext(final.EmergencyContact);
            isValid &= Validator.TryValidateObject(
                final.EmergencyContact,
                studentEmergencyContext,
                validationResults,
                validateAllProperties: true
            );
        }
        if (final.StudentProgram != null)
        {
            var studentProgramContext = new ValidationContext(final.StudentProgram);
            isValid &= Validator.TryValidateObject(
                final.StudentProgram,
                studentProgramContext,
                validationResults,
                validateAllProperties: true
            );
        }
        if (final.AcademicHistory != null)
        {
            var studentAcademicContext = new ValidationContext(final.AcademicHistory);
            isValid &= Validator.TryValidateObject(
                final.AcademicHistory,
                studentAcademicContext,
                validationResults,
                validateAllProperties: true
            );
        }
        if (final.Essays != null)
        {
            var studentEssaysContext = new ValidationContext(final.Essays);
            isValid &= Validator.TryValidateObject(
                final.Essays,
                studentEssaysContext,
                validationResults,
                validateAllProperties: true
            );
        }


        return isValid;
    }
    public async Task DownloadPdf(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var application = await GetById(id, context);

            // Download the PDF to the user's PC.
            // ...
            // ...
            // ...
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }

    }

    public async Task ApproveApplication(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {

            var application = await GetById(id, context);
            if (application == null) throw new Exception("No application could be found to approve.");
            if (application.ApprovedStatus == Status.Approved) return;

            // Update student application to be marked as accepted
            application.ApprovedStatus = Status.Approved;

            // Create new student user + Create user Identity for Authorization / Authentication
            CreateUserResultHelper user = await _userService.CreateUserByApplication(application, context);
            if (user.User == null || user.ApplicationUser == null) throw new Exception("Could not create user.");
            await context.SaveChangesAsync();

            // CREATE EMAIL | Email Subject | HTML Body
            var firstLastName = $"{application.StudentInfo.FirstName} {application.StudentInfo.LastName}";
            var htmlTemplatePath = Path.Combine(_environment.ContentRootPath, "Components", "Ui", "EmailTemplates", "Approved.html"); // Approved Email Template
            var baseUrl = _configuration["AppSettings:BaseUrl"]; //Gets the base URL of the website
            Console.WriteLine($"GENERATING TOKEN");
            Console.WriteLine($"Identity ID: {user.ApplicationUser.Id}");
            Console.WriteLine($"Security Stamp: {user.ApplicationUser.SecurityStamp}");
            Console.WriteLine($"Email: {user.ApplicationUser.Email}");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user.ApplicationUser!); // Create token for link
            // token = Uri.EscapeDataString(token); // Encodes the token
            var registrationLink = $"{baseUrl}/register?userId={user.User!.Id}&token={token}";
            var subject = "Welcome to CSU";
            var html = await File.ReadAllTextAsync(htmlTemplatePath); //Template for approved letters

            html = html.Replace("{{Link_To_Register}}", registrationLink);
            html = html.Replace("{{Student_First_And_Last_Name}}", firstLastName);

            // SEND NEW USER EMAIL TO REGISTER ACCOUNT
            await _emailService.SendEmailAsync(application.Email, firstLastName, subject, html);
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task DeclineApplication(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var application = await GetById(id, context);
            if (application == null) throw new Exception("No application could be found to deny.");

            // Mark application as denied
            application.ApprovedStatus = Status.Denied;

            // SAVE denied status
            await context.SaveChangesAsync();

            // Send email to notify student
            // CREATE EMAIL | Email Subject | HTML Body
            var firstLastName = $"{application.StudentInfo.FirstName} {application.StudentInfo.LastName}";
            var htmlTemplatePath = Path.Combine(_environment.ContentRootPath, "Components", "Ui", "EmailTemplates", "Denied.html");
            var subject = "CSU Admissions Decision";
            var html = await File.ReadAllTextAsync(htmlTemplatePath); //Template for approved letters

            html = html.Replace("{{student_name}}", firstLastName);

            // SEND NEW USER EMAIL TO REGISTER ACCOUNT
            await _emailService.SendEmailAsync(application.Email, firstLastName, subject, html);
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }

    public async Task PendingApplication(int id, ApplicationDbContext? context = null)
    {
        bool dispose = false;
        if (context == null)
        {
            context = await _context.CreateDbContextAsync();
            dispose = true;
        }
        try
        {
            var application = await GetById(id, context);
            if (application == null) throw new Exception("No application could be found to change to pending.");

            // Mark application as declined
            application.ApprovedStatus = Status.Pending;

            await context.SaveChangesAsync();
        }
        finally
        {
            if (dispose) await context.DisposeAsync();
        }
    }
}



