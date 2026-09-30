using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.Degrees;

public class Degree
{
    // EF Core Links
    public int Id { get; set; }

    // Degree Attributes
    [Required(ErrorMessage = "Degree name is required")]
    [StringLength(200, ErrorMessage = "Degree name cannot exceed 200 characters")]
    public string Name { get; set; } = null!;

    public List<Course> Courses { get; set; } = [];
    public List<UserProgramModel> StudentPrograms { get; set; } = [];

    [StringLength(2000, ErrorMessage = "Degree description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;


}





