using System.ComponentModel.DataAnnotations;

namespace GigaJira.Models.Entities;

public class Project
{
    public Guid Id { get; set; } // Primary key

    [Required(ErrorMessage = "Project name is required.")]
    [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
    public string ProjectName { get; set; } // Project name

    [Required(ErrorMessage = "Project description is required.")]
    public string Description { get; set; } // Project description

    // Foreign key for Owner
    public string OwnerId { get; set; } // The user who created the project
    public ApplicationUser Owner { get; set; } // Navigation property

    // Nullable Foreign Key for Organisation
    public Guid? OrganisationId { get; set; }

    // Navigation property for Organisation
    public Organisation? Organisation { get; set; }

    // Navigation property for users
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    public ICollection<Sprint> Sprints { get; set; } = new List<Sprint>(); // New
}
