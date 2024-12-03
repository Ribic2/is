using GigaJira.Data.Enums;
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

        // Configure the Project-Owner relationship
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner) // A project has one owner
            .WithMany() // The owner doesn't need a navigation property to projects
            .HasForeignKey(p => p.OwnerId) // Use OwnerId as the foreign key
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        // Configure Organisation-Users relationship
        modelBuilder.Entity<Organisation>()
            .HasMany(o => o.Users)
            .WithOne(u => u.Organisation)
            .HasForeignKey(u => u.OrganisationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Organisation-Projects relationship
        modelBuilder.Entity<Organisation>()
            .HasMany(o => o.Projects)
            .WithOne(p => p.Organisation)
            .HasForeignKey(p => p.OrganisationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Project-Users relationship
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Users)
            .WithMany(u => u.Projects);
        
        // Sprint-Project relationship
        modelBuilder.Entity<Sprint>()
            .HasOne<Project>() // Reference the Project entity
            .WithMany(p => p.Sprints) // A project can have many sprints
            .HasForeignKey(s => s.ProjectId) // Foreign Key
            .OnDelete(DeleteBehavior.Cascade);

        // Ticket-Sprint relationship
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Sprint)
            .WithMany()
            .HasForeignKey(t => t.SprintId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}