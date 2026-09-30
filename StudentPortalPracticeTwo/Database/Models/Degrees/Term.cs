using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Enums;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Term
{
    [Key]
    public int Id { get; set; }

    public ICollection<ClassSession> ClassSessions { get; set; } = [];

    [EnumDataType(typeof(TermSeason), ErrorMessage = "A valid term season is required")]
    public TermSeason Season { get; set; } // Fall or Spring. See Enum

    [Range(2000, 9999, ErrorMessage = "Term year must be between 2000 and 9999")]
    public int Year { get; set; }

    public string DisplayName => $"{Season} {Year}";

    public bool AvailableToRegisterClasses { get; set; } = false;
}