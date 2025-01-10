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
public class ProjectController: Controller
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }
    
    [HttpGet]
    public ViewResult CreateProject()
    {
        return View("CreateProject");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProject(CreateProjectViewModel createProjectViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("CreateProject");
        }
        
        var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

        var users = new List<ApplicationUser>();
        users.Add(user);
        
        var project = new Project{
                Description = createProjectViewModel.ProjectDescription,
                Name = createProjectViewModel.ProjectName,
                Creator = user,
                Users = users
        };
        
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return RedirectToAction("Project", "Project", new { id = project.ProjectId });
    }

    [HttpGet("/Projects/{id}")]
    public IActionResult Project(int id)
    {
        var project = _context.Projects
            .Include(p => p.TicketList)
            .FirstOrDefault(p => p.ProjectId == id);
        
        if (project == null)
        {
            return NotFound();
        }
        
        
        // Setup GroupBy list 
        var ticketsPerStatus = new List<KanbanViewModel>();
        foreach (var status in Enum.GetValues<StatusEnum>())
        {
            var ticketPerStatus = new KanbanViewModel();
            
            ticketPerStatus.Status = (StatusEnum)status;
            ticketPerStatus.Tickets = _context.Tickets.Where(
                t => t.Status == (StatusEnum)status
                )
                .Where(t => t.Project.ProjectId == id).ToList();
            
            ticketsPerStatus.Add(ticketPerStatus);
        }
        
        
        var viewModel = new ProjectViewModel
        {
            ProjectId = project.ProjectId,
            ProjectName = project.Name,
            TicketList = ticketsPerStatus
        };
        
        return View("Project", viewModel);
    }

    [HttpGet("/Projects/{id}/invite")]
    public IActionResult InvitePeopleToProject(int id)
    {
        var project = _context.Projects
            .Include(p => p.Users)
            .FirstOrDefault(p => p.ProjectId == id);

        if (project == null)
        {
            NotFound();
        }

        var usersNotInProject = _context.ApplicationUsers
            .Where(u => u.Projects.Any(p => p.ProjectId != project.ProjectId))
            .ToList();

        var invitePeopleToProject = new InvitePeopleToProjectViewModel();
        invitePeopleToProject.Users = usersNotInProject.ConvertAll(u =>
        {
            return new SelectListItem()
            {
                Text = u.Email,
                Value = u.Id,
                Selected = false
            };
        });
        invitePeopleToProject.ProjectId = project.ProjectId;
        
        return View("InvitePeople", invitePeopleToProject);
    }

    [HttpGet("/Projects/{id}/accept")]
    public async Task<IActionResult> AcceptProject(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

        if (project == null)
        {
            return NotFound();
        }

        if (user == null)
        {
            return NotFound();
        }
        
        var invite = _context.Invites.Where(i => i.Project == project)
            .Where(i => i.User == user)
            .First();
        
        project.Users.Add(user);

        invite.Accepted = true;
        _context.Invites.Update(invite);
        _context.SaveChanges();
        
        return Json(id);
    }
    
    [HttpPost("/Projects/{id}/invite")]
    public async Task<IActionResult> InvitePeopleToProject(InvitePeopleToProjectViewModel invitePeopleToProjectViewModel)
    {
        var project = _context.Projects.
            FirstOrDefault(p => p.ProjectId == invitePeopleToProjectViewModel.ProjectId);
        var userToAdd = _context.ApplicationUsers.FirstOrDefault(u =>
            u.Id != invitePeopleToProjectViewModel.UserId);


        if (project == null)
        {
            return NotFound();
        }
        
        if (userToAdd == null)
        {
            return NotFound();
        }

        var invite = new Invite();
        
        invite.Project = project;
        invite.User = userToAdd;
        
        _context.Invites.Add(invite);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Project", "Project",
            new { id = invitePeopleToProjectViewModel.ProjectId }
            );
    }
}