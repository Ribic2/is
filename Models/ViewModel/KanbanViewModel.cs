using Jira.Enums;
using Jira.Models.Entities;

namespace Jira.Models.ViewModel;

public class KanbanViewModel
{
    public StatusEnum Status { get; set; }
    public List<Ticket> Tickets { get; set; }
}