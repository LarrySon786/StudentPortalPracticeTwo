using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Components.Services.Students;
using StudentPortalPracticeTwo.Database;
using StudentPortalPracticeTwo.Database.Models.Application;

namespace StudentPortalPracticeTwo.Components.Services.Application;

public class FinalApplicationDb
{
    private readonly ApplicationDbContext _context;
    private readonly UserService _userService;

    public FinalApplicationDb(ApplicationDbContext context, UserService userService)
    {
        _context = context;
        _userService = userService;
    }


    // GET all applications (includes denied, approved, or pending)
    public async Task<List<ApplicationModel>> GetAllApplications()
    {
        return await _context.ApplicationDb
            .Include(x => x.StudentInfo)
            .Include(y => y.StudentContact)
            .ToListAsync();
    }

    // GET all PENDING applciations (applications that need reviewed and then approved by admins)
    public async Task<List<ApplicationModel>> GetAllPendingApplications()
    {
        return await _context.ApplicationDb
            .Where(x => x.ApprovedStatus == Status.Pending)
            .Include(x => x.StudentInfo)
            .Include(y => y.StudentContact)
            .ToListAsync();
    }

    // 

    public async Task<ApplicationModel?> GetByEmail(string email)
    {
        var entity = await _context.ApplicationDb
            .Include(x => x.StudentInfo)
            .Include(x => x.StudentContact)
            .FirstOrDefaultAsync(x => x.Email == email);

        return entity;
    }

    public async Task<ApplicationModel?> GetById(int id)
    {
        var entity = await _context.ApplicationDb
            .Include(x => x.StudentInfo)
            .Include(x => x.StudentContact)
            .FirstOrDefaultAsync(x => x.Id == id);

        return entity;
    }

    public async Task<ApplicationModel> CreateApplication(DraftApplicationModel draft)
    {
        // VALIDATE the Draft ApplicationModel Required fields first. Identify if any fields are NULL
        // To display validation errors, please use a try{} catch{} statement to catch and update UI.
        List<string> errors = new();

        ValidateRequiredFields(draft, errors);

        // SERVER SIDE VALIDATION
        //Declare a list to hold Validation errors 
        var validationResults = new List<ValidationResult>();
        ServerSideValidation(draft, validationResults);

        // Add server-side errors to UI errors
        errors.AddRange(validationResults
            .Where(v => !string.IsNullOrWhiteSpace(v.ErrorMessage))
            .Select(v => v.ErrorMessage!)
        );

        // BLOCK SAVE AND THROW ERRORS IF ANY
        if (errors.Any())
            throw new ValidationException(string.Join(Environment.NewLine, errors));

        // CREATE FINAL DRAFT FROM existing draft
        ApplicationModel entity = new()
        {
            Email = draft.Email,
            StudentInfo = new StudentInfoModel()
            {
                FirstName = draft.DraftStudentInfo!.FirstName!,
                LastName = draft.DraftStudentInfo!.LastName!,
            },
            StudentContact = new StudentContactModel()
            {
                Phone = draft.DraftStudentContact!.Phone!,
            },
            ApprovedStatus = Status.Pending
        };

        _context.ApplicationDb.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateApplication(ApplicationModel updated)
    {
        ApplicationModel? entity = await _context.ApplicationDb
            .Include(x => x.StudentContact)
            .Include(x => x.StudentInfo)
            .FirstOrDefaultAsync(x => x.Id == updated.Id);

        if (entity == null)
            throw new Exception("No application found");

        entity.Email = updated.Email;
        entity.StudentInfo.FirstName = updated.StudentInfo.FirstName;
        entity.StudentInfo.LastName = updated.StudentInfo.LastName;

        entity.StudentContact.Phone = updated.StudentContact.Phone;

        await _context.SaveChangesAsync();
    }

    public bool ValidateRequiredFields(DraftApplicationModel draft, List<string>? errors)
    {
        if (errors == null)
            errors = new();

        if (string.IsNullOrWhiteSpace(draft.Email))
            errors.Add("Please add an Email to your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.FirstName))
            errors.Add("Please add a First Name to your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentInfo?.LastName))
            errors.Add("Please add a Last Name to your application.");

        if (string.IsNullOrWhiteSpace(draft.DraftStudentContact?.Phone))
            errors.Add("Please add a Phone Number to your application.");

        if (errors.Count == 0)
            return true;

        return false;
    }

    public bool ServerSideValidation(DraftApplicationModel draft, List<ValidationResult>? validationResults)
    {
        if (validationResults == null)
            validationResults = new();

        bool isValid = true;

        // Validate parent
        var parentContext = new ValidationContext(draft);
        isValid &= Validator.TryValidateObject(
            draft,
            parentContext,
            validationResults,
            validateAllProperties: true
        );

        // Validate StudentInfo
        if (draft.DraftStudentInfo != null)
        {
            var studentInfoContext = new ValidationContext(draft.DraftStudentInfo);
            isValid &= Validator.TryValidateObject(
                draft.DraftStudentInfo,
                studentInfoContext,
                validationResults,
                validateAllProperties: true
            );
        }

        // Validate StudentContact
        if (draft.DraftStudentContact != null)
        {
            var studentContactContext = new ValidationContext(draft.DraftStudentContact);
            isValid &= Validator.TryValidateObject(
                draft.DraftStudentContact,
                studentContactContext,
                validationResults,
                validateAllProperties: true
            );
        }

        return isValid;
    }

    public async Task DownloadPdf(int id)
    {
        var application = await GetById(id);

        // Download the PDF to the user's PC.
        // ...
        // ...
        // ...

    }

    public async Task ApproveApplication(int id)
    {
        var application = await GetById(id);
        if (application == null) throw new Exception("No application could be found to approve.");

        // TODO - Create student login credentials

        // Create new student user.
        await _userService.CreateUserByApplication(application);

        // Update student application to be marked as accepted
        application.ApprovedStatus = Status.Approved;
        await _context.SaveChangesAsync();

        // Send an email to the student | Send login credentials and acceptance letter
        // var html = await File.ReadAllTextAsync("Components/Ui/EmailTemplates/Approved.html");

        // html = html.Replace("{{Student_First_And_Last_Name}}", $"{application.StudentInfo.FirstName} {application.StudentInfo.LastName}");
        // html = html.Replace("{{Link_To_Register}}", "New Link to Sign-up");
    }

    public async Task DeclineApplication(int id)
    {
        var application = await GetById(id);
        if (application == null) throw new Exception("No application could be found to deny.");

        // Mark application as declined
        application.ApprovedStatus = Status.Denied;

        // Send email to notify student
    }

    public async Task PendingApplication(int id)
    {
        var application = await GetById(id);
        if (application == null) throw new Exception("No application could be found to change to pending.");

        // Mark application as declined
        application.ApprovedStatus = Status.Pending;
    }
}



