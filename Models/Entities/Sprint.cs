namespace Jira.Models.Entities;

public class Sprint
{
    public int SprintID { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<Ticket> TicketList { get; set; }
}