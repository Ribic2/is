namespace GigaJira.Models.Entities;

public class Project
{
    public Guid Id { get; set; } // Primary key
    public string ProjectName { get; set; } // Project name
    public string Description { get; set; } // Project description

    // Foreign key for Organisation
    public Guid OrganisationId { get; set; }

    // Navigation property for Organisation
    public Organisation Organisation { get; set; }

    // Navigation property for users
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}