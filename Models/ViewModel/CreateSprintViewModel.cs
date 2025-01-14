using Microsoft.Build.Framework;

namespace Jira.Models.ViewModel;

public class CreateSprintViewModel
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    public int ProjectId { get; set; }
}