using Jira.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jira.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<Ticket> Tickets { get; set; }
    
    public DbSet<ApplicationUser> ApplicationUsers  { get; set; }
    
    public DbSet<Sprint> Sprints { get; set; }
    
    public DbSet<Invite> Invites { get; set; }

}