using Jira.Data;
using Jira.Models.Entities;
using Jira.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Jira.Controllers;

[Authorize]
public class SprintController:  Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public SprintController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpGet("/Projects/{id}/Sprint")]
    public IActionResult CreateSprint(int id)
    {
        CreateSprintViewModel model = new CreateSprintViewModel();
        model.ProjectId = id;
        
        return View("CreateSprint", model);
    }
    
    [HttpPost("/Projects/{id}/Sprint")]
    public async Task<IActionResult> CreateSprint(CreateSprintViewModel createSprintViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("CreateSprint", createSprintViewModel);
        }
        
        Sprint sprint = new Sprint();
        sprint.Name = createSprintViewModel.Name;
        sprint.Description = createSprintViewModel.Description;
        
        var project = _context.Projects.Find(createSprintViewModel.ProjectId);

        if (project == null)
        {
            return NotFound();
        }
        project.SprintList.Add(sprint);
        
        _context.Sprints.Add(sprint);
        _context.SaveChanges();
        
        return RedirectToAction("Project", "Project",
            new { id = createSprintViewModel.ProjectId }
        );
    }
}