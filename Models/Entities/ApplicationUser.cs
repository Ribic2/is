using Microsoft.AspNetCore.Identity;

namespace GigaJira.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public Guid? OrganisationId { get; set; }
    public Organisation Organisation { get; set; }
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}