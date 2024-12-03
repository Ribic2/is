using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigaJira.Models.Entities;

public class Sprint
{
    public Guid Id { get; set; } 

    [Required(ErrorMessage = "Sprint name is required.")]
    public string Name { get; set; } 

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; } 

    [Required(ErrorMessage = "End date is required.")]
    public DateTime EndDate { get; set; } 
    
    [Required(ErrorMessage = "The ProjectId is required.")]
    public Guid ProjectId { get; set; } 

    [NotMapped]
    public Project Project { get; set; } 
}
