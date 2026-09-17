using System.ComponentModel.DataAnnotations;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Enums;
using StudentPortalPracticeTwo.Database.Models.Users.Students;

namespace StudentPortalPracticeTwo.Database.Models.SupportTicket;

public class SupportTicket
{
    [Key]
    public int Id { get; set; }

    // Student reference
    [Required(ErrorMessage = "A student Id is required")]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    // Reference to Response
    public List<ResponseTicket> ResponseTicket { get; set; } = new(); // The student's questions, concerns, and other matters


    // Properties
    public SupportStatus Status { get; set; } = SupportStatus.Submitted; // Has this ticket been processed?

    [Required(ErrorMessage = "A support topic is required")]
    public SupportTopic Topic { get; set; } = SupportTopic.Other;

    [Required(ErrorMessage = "A title is required")]
    public required string Title { get; set; } = null!;



}