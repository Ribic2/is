using GigaJira.Data.Enums;
using GigaJira.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GigaJira.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Organisation> Organisations { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Organisation-Users relationship
        modelBuilder.Entity<Organisation>()
            .HasMany(o => o.Users) // An organisation has many users
            .WithOne(u => u.Organisation) // Each user belongs to one organisation
            .HasForeignKey(u => u.OrganisationId) // Foreign key in ApplicationUser
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Organisation-Projects relationship
        modelBuilder.Entity<Organisation>()
            .HasMany(o => o.Projects) // An organisation has many projects
            .WithOne(p => p.Organisation) // Each project belongs to one organisation
            .HasForeignKey(p => p.OrganisationId) // Foreign key in Project
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Project-Users relationship
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Users) // A project has many users
            .WithMany(u => u.Projects); // Many-to-many relationship
    }
}