using Microsoft.AspNetCore.Identity;

namespace Jira.Models.Entities;

public class ApplicationUser: IdentityUser
{
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}