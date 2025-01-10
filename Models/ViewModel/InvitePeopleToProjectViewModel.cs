using Jira.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jira.Models.ViewModel;

public class InvitePeopleToProjectViewModel
{
    public List<SelectListItem> Users { get; set; }
    public string UserId { get; set; }
    public int ProjectId { get; set; }
}