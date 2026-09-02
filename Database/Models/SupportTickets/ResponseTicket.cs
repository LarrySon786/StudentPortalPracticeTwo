using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Users;


namespace StudentPortalPracticeTwo.Database.Models.SupportTicket;

public class ResponseTicket
{
    [Key]
    public int Id { get; set; }

    // Ticket reference
    public int SupportTicketId { get; set; }
    public SupportTicket? SupportTicket { get; set; }

    // Author reference
    public int UserId{ get; set; } // This is the author of this response (instructor or student)
    public UserModel? User { get; set; }

    // Properties
    [Required(ErrorMessage = "Support ticket input is required")]
    public string StudentTicketInput { get; set; } = null!; // The student's questions, concerns, and other matters



}