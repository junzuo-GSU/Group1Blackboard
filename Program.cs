using Group1Blackboard.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// session needs a cache and the session service (Chapter 9)
builder.Services.AddMemoryCache();
builder.Services.AddSession();

// EF Core with SQLite; the connection string is in appsettings.json
builder.Services.AddDbContext<BlackboardContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BlackboardContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// must come after UseRouting and before the endpoints that use the session
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

// map route for Admin area - the URL must start with "Admin"
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// custom route that carries the catalog filters in the URL, must come before the default route
app.MapControllerRoute(
    name: "custom",
    pattern: "{controller}/{action}/major-{activeMajor}/loc-{activeLoc}/sem-{activeSem}")
    .WithStaticAssets();

// map default route for the main (student) area
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
