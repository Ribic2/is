using System.Text.Json;
using Jira.Controllers.dtos;
using Jira.Data;
using Jira.Filters;
using Jira.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jira.Controllers;

[Route("api/[controller]")]
[ApiKeyAuth]
[ApiController]
public class ProjectApiController
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ProjectApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpGet("projects")]
    public async Task<IActionResult> GetProjects()
    {
        var projects = _context.Projects
            .Include(p => p.TicketList) // Ensure related data is loaded
            .Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId,
                Name = p.Name,
                Description = p.Description,
                Tickets = p.TicketList.Select(t => new TicketDto
                {
                    TicketId = t.TicketID,
                    TicketName = t.TicketName,
                    TitleDescription = t.TicketDescription,
                    Status = t.Status
                }).ToList()
            })
            .ToList();
        
        
        
        return new JsonResult(projects);
    }
    
    [HttpGet("project/{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = _context.Projects.Where(p => p.ProjectId == id)
            .Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId,
                Name = p.Name,
                Tickets = p.TicketList.Select(t => new TicketDto
                {
                    TicketId = t.TicketID,
                    TicketName = t.TicketName,
                    TitleDescription = t.TicketDescription,
                    Status = t.Status,
                }).ToList()
            });
        
        return new JsonResult(project);
    }
}