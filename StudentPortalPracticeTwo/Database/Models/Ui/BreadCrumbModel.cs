namespace StudentPortalPracticeTwo.Database.Models.Ui;

public class BreadCrumbModel
{
    public string Name { get; set; } = string.Empty;
    public string? Link { get; set; }
    public bool IsCurrent { get; set; } = false;
}