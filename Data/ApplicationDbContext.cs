using GigaJira.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GigaJira.Data;

public class ApplicationDbContext :  IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Sprint> Sprints { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner) 
            .WithMany() 
            .HasForeignKey(p => p.OwnerId) 
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Organisation>()
            .HasMany(o => o.Users)
            .WithOne(u => u.Organisation)
            .HasForeignKey(u => u.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Organisation>()
            .HasMany(o => o.Projects)
            .WithOne(p => p.Organisation)
            .HasForeignKey(p => p.OrganisationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Users)
            .WithMany(u => u.Projects);
        
        modelBuilder.Entity<Sprint>()
            .HasOne<Project>() 
            .WithMany(p => p.Sprints) 
            .HasForeignKey(s => s.ProjectId) 
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Sprint)
            .WithMany()
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}