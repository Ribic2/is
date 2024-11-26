using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GigaJira.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using GigaJira.Data;
using GigaJira.Models.Entities;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
        builder.Configuration.GetConnectionString("ApplicationDbContextConnection")
    )
);



builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("keys"))
    .SetApplicationName("YourAppName");

// Add Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // Disable confirmation for login
        options.SignIn.RequireConfirmedEmail = false; // Disable email confirmation
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
app.MapRazorPages();


// Run the seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    await Seeder.SeedAsync(context);
}

app.Run();
