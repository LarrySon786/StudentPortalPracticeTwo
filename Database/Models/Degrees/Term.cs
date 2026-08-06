using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Term
{
    [Key]
    public int Id { get; set; }

    public ICollection<ClassSession> ClassSessions { get; set; } = [];

    public TermSeason Season { get; set; } // Fall or Spring. See Enum
    public int Year { get; set; }

    public string DisplayName => $"{Season} {Year}";

    public bool AvailableToRegisterClasses { get; set; } = false;
}