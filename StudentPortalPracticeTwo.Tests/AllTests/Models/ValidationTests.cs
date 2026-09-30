using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Degrees;

namespace StudentPortalPracticeTwo.Tests.AllTests.Models;

public class ValidationTests
{
    [Fact]
    public void StudentEssay_RejectsResponsesOutsideAllowedLength()
    {
        var essay = new StudentEssayModel
        {
            ResponseOne = new string('a', 299),
            ResponseTwo = new string('a', 3001),
            ResponseThree = new string('a', 300)
        };

        var results = Validate(essay);

        Assert.Contains(results, result => result.ErrorMessage == "Essay response one must be at least 300 characters");
        Assert.Contains(results, result => result.ErrorMessage == "Essay response two cannot exceed 3000 characters");
    }

    [Fact]
    public void Course_RejectsMissingRequiredFields()
    {
        var results = Validate(new Course());

        Assert.Contains(results, result => result.ErrorMessage == "Course code is required");
        Assert.Contains(results, result => result.ErrorMessage == "Course name is required");
    }

    [Fact]
    public void DraftStudentInfo_DoesNotRequireOptionalFields()
    {
        var results = Validate(new DraftStudentInfoModel());

        Assert.Empty(results);
    }

    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }
}