using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigaJira.Models.Entities;

public class Sprint
{
    public Guid Id { get; set; } // Primary Key

    [Required(ErrorMessage = "Sprint name is required.")]
    public string Name { get; set; } // Sprint name

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } // Sprint start date

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; } // Sprint end date
    
    [Required(ErrorMessage = "The ProjectId is required.")]
    public Guid ProjectId { get; set; } // Foreign Key for Project

    [NotMapped]
    public Project Project { get; set; } // Navigation property (optional)
}
