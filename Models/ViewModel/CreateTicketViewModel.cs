using Jira.Enums;

namespace Jira.Models.ViewModel;

public class CreateTicketViewModel
{
    public int ProjectId { get; set; }
    public string TicketName { get; set; }

    public string TicketDescription { get; set; }
    
}