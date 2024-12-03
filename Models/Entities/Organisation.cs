namespace GigaJira.Models.Entities;

public class Organisation
{
    public Guid Id { get; set; } // Primary key
    public string OrganisationName { get; set; } // Organisation name
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}