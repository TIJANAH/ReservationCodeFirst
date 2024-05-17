global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyReservations.Interfaces;
using MyReservations.Models;
using MyReservations.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ReservationsContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SQLDataReservations")));

builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<TokenService>();

var appConfig = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(appConfig);
// Using AppSettings
var appSettings = appConfig.Get<JwtSettings>();
var secret = Encoding.ASCII.GetBytes(appSettings.Key);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(option =>
//                {
//                    option.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuerSigningKey = true,
//                        IssuerSigningKey = new SymmetricSecurityKey(
//                            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Token").Value!)),
//                        ValidateIssuer = true,
//                        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
//                        ValidateAudience = true,
//                        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value
//                    };
//                });


builder.Services.AddJwtTokenConfiguration(secret);
var app = builder.Build();


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
app.UseAuthentication();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();