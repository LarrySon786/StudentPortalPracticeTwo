
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public static class GraduationDateHelper
{
    // This function predicts graduation dates
    public static DateOnly PredictGraduationDate(UserProgramModel program, Degree degree)
    {
        int totalCredits = degree.Courses.Sum(x => x.Credits);
        int totalComplete = program.CompletedCourses.Sum(x => x.Course.Credits);

        if (totalComplete >= totalCredits)
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }

        int creditsRemaining = totalCredits - totalComplete;
        int termsRemaining = (int)Math.Ceiling((double)creditsRemaining / 15);
        int daysExpectedForGraduation = termsRemaining * 180;

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        var estimated = DateOnly.FromDateTime(DateTime.Now.AddDays(daysExpectedForGraduation));

        if (estimated >= new DateOnly(estimated.Year, 5, 20) && estimated <= new DateOnly(estimated.Year, 12, 31))
        {
            return new DateOnly(estimated.Year, 12, 20);

        }
        else if (estimated <= new DateOnly(estimated.Year, 5, 19) && estimated >= new DateOnly(estimated.Year, 1, 1))
        {
            return new DateOnly(estimated.Year, 5, 20);
        }
        else
        {
            return new DateOnly(estimated.Year, 12, 20);
        }
    }
}