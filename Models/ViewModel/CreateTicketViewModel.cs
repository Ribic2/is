using Jira.Enums;
using Microsoft.Build.Framework;

namespace Jira.Models.ViewModel;

public class CreateTicketViewModel
{
    public int ProjectId { get; set; }
    [Required]
    public string TicketName { get; set; }

    [Required]
    public string TicketDescription { get; set; }
    
}