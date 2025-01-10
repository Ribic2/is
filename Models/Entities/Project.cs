using Microsoft.AspNetCore.Identity;

namespace Jira.Models.Entities;

public class Project
{
    public int ProjectId { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public IdentityUser Creator { get; set; }

    public ICollection<Ticket> TicketList { get; set; }

    public ICollection<Sprint> SprintList { get; set; } = new List<Sprint>();

    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}