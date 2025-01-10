using Jira.Data;
using Jira.Filters;
using Jira.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jira.Controllers;

[Route("api")]
[ApiKeyAuth]
public class ApiController
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpGet("projects")]
    public async Task<IActionResult> GetProjects()
    {
        var projects = _context.Projects.Include(
            p => p.TicketList
            ).ToList();
        return new JsonResult(projects);
    }
    
    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = _context.Tickets.ToList();
        return new JsonResult(tickets);
    }
    
    [HttpGet("project/{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = _context.Projects.FirstOrDefault(p=>p.ProjectId == id);
        return new JsonResult(project);
    }
}