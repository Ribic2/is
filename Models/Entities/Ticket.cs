using GigaJira.Data.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GigaJira.Models.Entities;

public class Ticket
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "The Ticket Name is required.")]
    public string TicketName { get; set; }

    public string TicketDescription { get; set; }

    public StatusEnum? Status { get; set; }

    [Required(ErrorMessage = "The Project field is required.")]
    public Guid ProjectId { get; set; }

    [ValidateNever] // Avoid validation for navigation properties
    public Project Project { get; set; }

    public Guid? SprintId { get; set; }

    [ValidateNever] // Avoid validation for navigation properties
    public Sprint Sprint { get; set; }

    public string AssigneId { get; set; }

    [ValidateNever] // Avoid validation for navigation properties
    public ApplicationUser Assigne { get; set; }

    public string ApproverId { get; set; }

    [ValidateNever] // Avoid validation for navigation properties
    public ApplicationUser Approver { get; set; }
}
