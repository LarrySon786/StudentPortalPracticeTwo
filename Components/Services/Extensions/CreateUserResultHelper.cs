using StudentPortalPracticeTwo.Database.Models.Students;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public class CreateUserResultHelper
{
    public UserModel? User { get; set; } = null;
    public ApplicationUser? ApplicationUser { get; set; } = null;
}