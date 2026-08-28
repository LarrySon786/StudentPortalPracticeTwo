using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Grade
{
    [Key]
    public int Id { get; set; }

    // Linked Student
    public UserProgramModel StudentProgram { get; set; } = null!;
    public int StudentProgramId { get; set; }

    // Linked Assignment
    public Assignments Assignment { get; set; } = null!;
    public int AssignmentId { get; set; }

    // Linked Class Session
    public ClassSession Session { get; set; } = null!;
    public int SessionId { get; set; }

    // Properties
    public int ScoredPoints { get; set; }
    public decimal PercentageGrade => Math.Round((decimal)(ScoredPoints / Assignment.TotalPoints), 2);
}