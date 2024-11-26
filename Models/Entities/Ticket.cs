namespace GigaJira.Models.Entities;

using Microsoft.AspNetCore.Identity;

public class Ticket
{
    public Guid Id { get; set; }
    public Project Project { get; set; }
    public string ticketName { get; set; }
    public string ticketDescription { get; set; }
    public Status? status { get; set; }
    public IdentityUser assigne { get; set; }
    public IdentityUser approver { get; set; }
}