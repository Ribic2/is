using GigaJira.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GigaJira.Data;

public class ApplicationDbContext: IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {
        
    }
    
    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Status> Status { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
}