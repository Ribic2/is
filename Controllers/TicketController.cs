using Humanizer;
using Jira.Data;
using Jira.Enums;
using Jira.Models.Entities;
using Jira.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Jira.Controllers;

[Authorize]
public class TicketController:  Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    
    public TicketController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public ViewResult CreateTicket(int projectId)
    {
        ViewData["ProjectId"] = projectId;
        return View("CreateTicket");
    }

    [HttpPost("/ticket/update")]
    public async Task<IActionResult> UpdateTicket(Ticket ticket)
    {
        var ticketToUpdate = _context.Tickets.Include(t => t.Project)
            .First(t => t.TicketID == ticket.TicketID);
        
        _context.Entry(ticketToUpdate).CurrentValues.SetValues(ticket);
        
        
        _context.SaveChanges();
        return RedirectToAction("Project", "Project",
            new { id = ticketToUpdate.Project.ProjectId }
        );
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTicket(CreateTicketViewModel createTicketViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("CreateTicket");
        }
        
        
        var project = await _context.Projects.FindAsync(createTicketViewModel.ProjectId);
        var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
        var ticket = new Ticket
        {
            Project = project,
            Reviewer = user,
            TicketName = createTicketViewModel.TicketName,
            TicketDescription = createTicketViewModel.TicketDescription
        };
        
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Project", "Project", new { id = project.ProjectId });
    }

    [HttpGet]
    public async Task<IActionResult> GetTicket(int id)
    {
        var ticket = _context.Tickets
            .Where(t => t.TicketID == id)
            .Include(ticket => ticket.Project)
            .Include(t => t.Project.Users)
            .FirstOrDefault();

        if (ticket == null)
        {
            return NotFound();
        }

        var sprints = _context.Projects
            .Where(p => p.ProjectId == ticket.Project.ProjectId)
            .Include(p => p.SprintList).First();
        
        ViewBag.Statuses = Enum.GetValues<StatusEnum>().Select(status => new SelectListItem
        {
            Text = status.ToString(),
            Value = ((int)status).ToString()
        });

        
        ViewBag.ActiveUsers = ticket.Project.Users.Select(projectUser => new SelectListItem()
        {
            Text = projectUser.Email,
            Value = projectUser.Id.ToString()
        });

        ViewBag.Sprints = sprints.SprintList.Select(sprint => new SelectListItem()
        {
            Text = sprint.Name,
            Value = sprint.SprintID.ToString()
        });
        
        return PartialView("_TicketDetailPartial" , ticket);
    }
    
    [HttpPost("tickets/{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets
            .Include(p=>p.Project)
            .Where(t => t.TicketID == id).FirstAsync();
        
        _context.Tickets.Remove(ticket);
        _context.SaveChangesAsync();

        return RedirectToAction("Project", "Project",
            new { id = ticket.Project.ProjectId }
        );
    }
}