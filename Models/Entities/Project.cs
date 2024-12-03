using System.ComponentModel.DataAnnotations;

namespace GigaJira.Models.Entities;

public class Project
{
    public Guid Id { get; set; } // Primary key

    [Required(ErrorMessage = "Project name is required.")]
    [StringLength(100, ErrorMessage = "Project name cannot exceed 100 characters.")]
    public string ProjectName { get; set; } 

    [Required(ErrorMessage = "Project description is required.")]
    public string Description { get; set; } 

    public string OwnerId { get; set; } 
    public ApplicationUser Owner { get; set; }

    public Guid? OrganisationId { get; set; }

    public Organisation? Organisation { get; set; }

    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    public ICollection<Sprint> Sprints { get; set; } = new List<Sprint>(); // New
}
