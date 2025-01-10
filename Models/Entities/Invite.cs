namespace Jira.Models.Entities;

public class Invite
{
    public int Id { get; set; }
    
    public ApplicationUser User { get; set; }
    
    public Project Project { get; set; }

    public bool Accepted { get; set; } = false;
}