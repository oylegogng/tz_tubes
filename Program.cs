using Microsoft.EntityFrameworkCore;
using tz_tubes;
using tz_tubes.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Server = LAPTOP-FU2ARVTC\\SQLEXPRESS02;User = oleg; Password = 123; Database = TubeDatabase; TrustServerCertificate = True; ");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "addpipe",
    pattern: "{controller=Home}/{action=AddPipe}/{id?}");

app.MapControllerRoute(
    name: "deletepipe",
    pattern: "deletepipe/{id?}",
    defaults: new { controller = "Home", action = "DeletePipe" }
);




app.Run();
