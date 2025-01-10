using System.Diagnostics;
using Jira.Data;
using Microsoft.AspNetCore.Mvc;
using Jira.Models;
using Jira.Models.Entities;
using Jira.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jira.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    public IActionResult Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel();

        var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

        homeViewModel.Projects = _context.Projects.
            Where(p => p.Users.Contains(user))
            .ToList();

        homeViewModel.Invites = _context.Invites.Where(i => i.User == user)
            .Where(i => i.Accepted == false)
            .Include(i => i.Project).ToList();
        
        return View(homeViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}