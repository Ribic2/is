namespace GigaJira.Models.Entities;

public class Organisation
{
    public Guid Id { get; set; } // Primary key
    public string OrganisationName { get; set; } // Organisation name

    // Navigation property for users in this organisation
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    // Navigation property for projects in this organisation
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}