
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public class TermServiceHelper
{


    public List<Term> CalculateUpcomingTerms() // The purpose of this function is to calculate the next TWO future terms for student applicants
    {
        List<Term> terms = new();

        int year = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;

        Term termOne = new();
        Term termTwo = new();

        if (currentMonth < 8) // Sets terms dates / seasons
        {
            termOne.Year = year;
            termOne.Season = TermSeason.Fall;
            termTwo.Year = year + 1;
            termTwo.Season = TermSeason.Spring;
        }
        else
        {
            termOne.Year = year + 1;
            termOne.Season = TermSeason.Spring;
            termTwo.Year = year + 1;
            termTwo.Season = TermSeason.Fall;
        }

        terms.Add(termOne);
        terms.Add(termTwo);
        return terms;
    }
}
