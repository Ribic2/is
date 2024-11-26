namespace GigaJira.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using GigaJira.Data.Enums;
using System.Threading.Tasks;
using GigaJira.Models.Entities;
using Microsoft.EntityFrameworkCore;

public static class Seeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
{
    // Ensure the database is created
    await context.Database.MigrateAsync();

    // Seed Organisations
    if (!context.Organisations.Any())
    {
        var organisations = new List<Organisation>
        {
            new Organisation
            {
                Id = Guid.NewGuid(),
                OrganisationName = "Organisation A"
            },
            new Organisation
            {
                Id = Guid.NewGuid(),
                OrganisationName = "Organisation B"
            }
        };

        await context.Organisations.AddRangeAsync(organisations);
        await context.SaveChangesAsync(); // Save here before seeding Projects
    }

    // Seed Projects
    if (!context.Projects.Any())
    {
        var organisationA = context.Organisations.FirstOrDefault(o => o.OrganisationName == "Organisation A");
        var organisationB = context.Organisations.FirstOrDefault(o => o.OrganisationName == "Organisation B");

        if (organisationA == null || organisationB == null)
        {
            throw new InvalidOperationException("Organisations must exist before seeding Projects.");
        }

        var projects = new List<Project>
        {
            new Project
            {
                Id = Guid.NewGuid(),
                ProjectName = "Project Alpha",
                Description = "Description for Project Alpha",
                OrganisationId = organisationA.Id // Reference Organisation A
            },
            new Project
            {
                Id = Guid.NewGuid(),
                ProjectName = "Project Beta",
                Description = "Description for Project Beta",
                OrganisationId = organisationB.Id // Reference Organisation B
            }
        };

        await context.Projects.AddRangeAsync(projects);
        await context.SaveChangesAsync(); // Save Projects
    }

    // Seed Statuses
    if (!context.Statuses.Any())
    {
        var statuses = new List<Status>
        {
            new Status { Id = Guid.NewGuid(), status = StatusEnum.IN_PROGRESS },
            new Status { Id = Guid.NewGuid(), status = StatusEnum.BACKLOG },
            new Status { Id = Guid.NewGuid(), status = StatusEnum.DONE },
            new Status { Id = Guid.NewGuid(), status = StatusEnum.QA },
            new Status { Id = Guid.NewGuid(), status = StatusEnum.WORKING_ON }
        };

        await context.Statuses.AddRangeAsync(statuses);
        await context.SaveChangesAsync(); // Save Statuses
    }
}

}
