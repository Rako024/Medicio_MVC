using Business.Services.Abstracts;
using Business.Services.Concretes;
using Core.Models;
using Core.RepositoryAbstracts;
using Data.DAL;
using Data.RepositoryConcretes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    //opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Asus"));
});

var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Account}/{action=Login}"
    );
app.Run();
