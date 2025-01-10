using Jira.Models.Entities;

namespace Jira.Models.ViewModel;

public class HomeViewModel
{
    public List<Project> Projects { get; set; } = new List<Project>();
    
    public List<Invite> Invites { get; set; } = new List<Invite>();
}