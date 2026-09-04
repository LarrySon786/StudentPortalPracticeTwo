

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public static class CalculateGPAHelper
{
    public static double CalculateGPA(decimal percent) // Return GPA based on percentage grade
    {
        var percentGrade = (double)percent;
        if (percentGrade >= 0.93)
        {
            return 4.00;
        }
        else if (percentGrade >= 0.90)
        {
            return 3.70;
        }
        else if (percentGrade >= 0.87)
        {
            return 3.30;
        }
        else if (percentGrade >= 0.83)
        {
            return 3.00;
        }
        else if (percentGrade >= 0.80)
        {
            return 2.70;
        }
        else if (percentGrade >= 0.77)
        {
            return 2.30;
        }
        else if (percentGrade >= 0.73)
        {
            return 2.00;
        }
        else if (percentGrade >= 0.70)
        {
            return 1.70;
        }
        else if (percentGrade >= 0.67)
        {
            return 1.30;
        }
        else if (percentGrade >= 0.63)
        {
            return 1.00;
        }
        else if (percentGrade >= 0.60)
        {
            return 0.70;
        }
        else
        {
            return 0.00;
        }
    }

    public static double CalculateAverageGPA(List<decimal> GPAs) // Returns average GPA for all a student's courses
    {
        double totalSum = (double)GPAs.Sum();
        double totalGPAs = GPAs.Count();
        return totalSum / totalGPAs;
    }
}