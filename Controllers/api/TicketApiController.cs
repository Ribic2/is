using Jira.Data;
using Jira.Filters;
using Jira.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jira.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiKeyAuth]
public class TicketApiController
{
    
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public TicketApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = _context.Tickets.ToList();
        return new JsonResult(tickets);
    }
}