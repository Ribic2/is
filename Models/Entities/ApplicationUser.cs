using Microsoft.AspNetCore.Identity;

namespace GigaJira.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public Guid? OrganisationId { get; set; }

    // Navigation property for Organisation
    public Organisation Organisation { get; set; }

    // Navigation property for projects
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}