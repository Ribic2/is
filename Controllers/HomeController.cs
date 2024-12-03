using System.Diagnostics;
using System.Security.Claims;
using GigaJira.Data;
using GigaJira.Data.Enums;
using GigaJira.Data.Extension;
using Microsoft.AspNetCore.Mvc;
using GigaJira.Models;
using GigaJira.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GigaJira.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _context.Users
            .Include(u => u.Projects) // Include the related projects
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        // Return the projects to the view
        return View(user.Projects);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        // Get the currently logged-in user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Ensure the user is logged in
        if (string.IsNullOrEmpty(userId))
        {
            return Redirect("/Identity/Account/Login");
        }

        // Fetch the user from the database
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        // Programmatically assign OwnerId and Owner
        project.OwnerId = userId;
        project.Owner = user;

        // Remove OwnerId and Owner from ModelState validation
        ModelState.Remove(nameof(Models.Entities.Project.OwnerId));
        ModelState.Remove(nameof(Models.Entities.Project.Owner));

        if (ModelState.IsValid)
        {
            // Add the owner to the project's Users collection
            project.Users.Add(user);

            // Add the project to the database
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Redirect to the list of projects
            return RedirectToAction("Index");
        }

        // Log ModelState errors for debugging
        foreach (var key in ModelState.Keys)
        {
            var state = ModelState[key];
            foreach (var error in state.Errors)
            {
                _logger.LogError($"Key: {key}, Error: {error.ErrorMessage}");
            }
        }

        return View(project); // Redisplay the form with errors
    }


    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Project(Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Fetch the user and check project access
        var user = await _context.Users
            .Include(u => u.Projects)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null || !user.Projects.Any(p => p.Id == projectId))
        {
            return RedirectToAction("Index");
        }

        // Fetch the project and include related entities
        var project = await _context.Projects
            .Include(p => p.Tickets)
            .ThenInclude(t => t.Assigne)
            .Include(p => p.Tickets)
            .ThenInclude(t => t.Approver)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
        {
            return RedirectToAction("Index");
        }

        // Define all possible statuses
        var allStatuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>();

        // Group tickets by status
        var groupedTickets = allStatuses
            .ToDictionary(
                status => status.GetDescription(),
                status => project.Tickets
                    .Where(t => t.Status == status)
                    .AsEnumerable()
            );

        ViewBag.GroupedTickets = groupedTickets;

        // Pass statuses for the form dropdown
        ViewBag.Statuses = allStatuses
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDescription()
            })
            .ToList();

        return View(project);
    }


    [HttpGet]
    public async Task<IActionResult> CreateTicket(Guid projectId)
    {
        ViewBag.ProjectId = projectId;

        // Fetch all users for dropdowns
        ViewBag.Users = await _context.Users
            .Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            })
            .ToListAsync();

        // Fetch statuses
        ViewBag.Statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDescription()
            }).ToList();

        // Fetch sprints for the project
        ViewBag.Sprints = await _context.Sprints
            .Where(s => s.ProjectId == projectId)
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(Guid projectId, Ticket ticket)
    {
        // Debug received ticket data
        _logger.LogInformation("Received Ticket Data: {@Ticket}", ticket);

        if (!ModelState.IsValid)
        {
            // Log validation errors
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    _logger.LogError($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }

            // Reload dropdowns for validation failure
            ViewBag.ProjectId = projectId;
            ViewBag.Users = await _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.UserName
                })
                .ToListAsync();

            ViewBag.Statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.GetDescription()
                }).ToList();

            ViewBag.Sprints = await _context.Sprints
                .Where(s => s.ProjectId == projectId)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToListAsync();

            return View(ticket);
        }

        ticket.ProjectId = projectId;

        // Handle optional fields
        if (string.IsNullOrWhiteSpace(ticket.SprintId.ToString()))
        {
            ticket.SprintId = null;
        }
        if (string.IsNullOrWhiteSpace(ticket.AssigneId))
        {
            ticket.AssigneId = null;
        }
        if (string.IsNullOrWhiteSpace(ticket.ApproverId))
        {
            ticket.ApproverId = null;
        }

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Ticket created successfully: {@Ticket}", ticket);

        return RedirectToAction("Project", new { projectId });
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> TicketDetails(Guid ticketId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Assigne)
            .Include(t => t.Approver)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
        {
            return NotFound("Ticket not found.");
        }

        // Populate users for the dropdown
        ViewBag.Users = await _context.Users
            .Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            })
            .ToListAsync();

        // Populate statuses for the dropdown
        ViewBag.Statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>()
            .Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDescription()
            })
            .ToList();

        return PartialView("_TicketDetailsPartial", ticket);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTicket(Ticket ticket)
    {
        // Log incoming data
        _logger.LogInformation("Incoming ticket data: {@Ticket}", ticket);

        if (!ModelState.IsValid)
        {
            // Collect validation errors
            var errors = ModelState.Keys
                .SelectMany(key => ModelState[key].Errors.Select(x => new { Key = key, Error = x.ErrorMessage }))
                .ToList();

            // Log validation errors
            foreach (var error in errors)
            {
                _logger.LogError($"Key: {error.Key}, Error: {error.Error}");
            }

            return Json(new { success = false, errors });
        }

        var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
        if (existingTicket == null)
        {
            return Json(new { success = false, message = "Ticket not found." });
        }

        // Update the ticket
        existingTicket.TicketName = ticket.TicketName;
        existingTicket.TicketDescription = ticket.TicketDescription;
        existingTicket.Status = ticket.Status;
        existingTicket.AssigneId = ticket.AssigneId;
        existingTicket.ApproverId = ticket.ApproverId;

        await _context.SaveChangesAsync();

        // Log successful update
        _logger.LogInformation("Ticket updated successfully: {@Ticket}", existingTicket);

        return Json(new { success = true, message = "Ticket updated successfully." });
    }


    /**
     * Sprint part
     */
    [HttpGet]
    public IActionResult CreateSprint(Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return Unauthorized("You are not authorized to create sprints for this project.");
        }

        var sprint = new Sprint
        {
            ProjectId = projectId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(14)
        };

        return View(sprint);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSprint(Sprint sprint)
    {
        _logger.LogInformation("Received ProjectId: {ProjectId}", sprint.ProjectId);

        ModelState.Remove(nameof(Sprint.Project)); // Remove validation for navigation property

        if (!ModelState.IsValid)
        {
            // Log validation errors
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    _logger.LogError($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }

            return View(sprint);
        }

        // Ensure the project exists
        var project = await _context.Projects.FindAsync(sprint.ProjectId);
        if (project == null)
        {
            ModelState.AddModelError("ProjectId", "The specified project does not exist.");
            return View(sprint);
        }

        // Add and save sprint
        sprint.Project = project; // Explicitly set the project for the sprint
        _context.Sprints.Add(sprint);
        await _context.SaveChangesAsync();

        return RedirectToAction("Project", new { projectId = sprint.ProjectId });
    }


    [HttpGet]
    public async Task<IActionResult> AssignSprint(Guid ticketId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Project)
            .Include(t => t.Sprint)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
        {
            return NotFound("Ticket not found.");
        }

        ViewBag.Sprints = await _context.Sprints
            .Where(s => s.ProjectId == ticket.ProjectId)
            .Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            })
            .ToListAsync();

        return View(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> AssignSprint(Guid ticketId, Guid sprintId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        if (ticket == null)
        {
            return NotFound("Ticket not found.");
        }

        var sprint = await _context.Sprints.FindAsync(sprintId);
        if (sprint == null)
        {
            return NotFound("Sprint not found.");
        }

        ticket.SprintId = sprintId;
        await _context.SaveChangesAsync();

        return RedirectToAction("Project", new { projectId = ticket.ProjectId });
    }


    /**
     * Invite user
     */
    [HttpGet]
    public async Task<IActionResult> InviteUser(Guid projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

        if (project == null)
        {
            return Unauthorized("You are not authorized to invite users to this project.");
        }

        ViewBag.ProjectId = projectId;
        ViewBag.Users = await _context.Users
            .Where(u => !project.Users.Contains(u)) // Exclude users already in the project
            .Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = u.UserName
            })
            .ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> InviteUser(Guid projectId, string userId)
    {
        var project = await _context.Projects
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
        {
            return NotFound("Project not found.");
        }

        if (project.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            return Unauthorized("You are not authorized to invite users to this project.");
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        project.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Project", new { projectId });
    }
}