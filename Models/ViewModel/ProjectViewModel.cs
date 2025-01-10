using Jira.Models.Entities;

namespace Jira.Models.ViewModel;

public class ProjectViewModel
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public List<KanbanViewModel> TicketList { get; set; }
}