using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GigaJira.Data;
using GigaJira.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GigaJira.Areas.Identity.Pages.Account.Manage
{
    public class OrganisationsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public OrganisationsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Organisation> Organisations { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            Organisations = _context.Organisations
                .Where(o => o.Users.Any(u => u.Id == user.Id))
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostLeaveAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var organisation = _context.Organisations
                .FirstOrDefault(o => o.Id == id);

            if (organisation == null)
            {
                return NotFound("Organisation not found.");
            }

            organisation.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}