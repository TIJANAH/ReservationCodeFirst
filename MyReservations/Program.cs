global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyReservations.Interfaces;
using MyReservations.Models;
using MyReservations.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ReservationsContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SQLDataReservations")));

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

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

app.Run();