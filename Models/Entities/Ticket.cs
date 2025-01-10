using Jira.Enums;
using Microsoft.AspNetCore.Identity;

namespace Jira.Models.Entities;

public class Ticket
{
    public int TicketID { get; set; }
    
    public string TicketName { get; set; }
    
    public string TicketDescription { get; set; }
    
    public IdentityUser Reviewer { get; set; }
    
    public IdentityUser Assigner { get; set; }

    public StatusEnum Status { get; set; } = StatusEnum.BACKLOG;
    public Project Project { get; set; }
    
    public Sprint? Sprint { get; set; }
}