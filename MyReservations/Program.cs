global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyReservations.Interfaces;
using MyReservations.Models;
using MyReservations.Services;
using System.Data;
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

builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.AllowedUserNameCharacters += " ";
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<ReservationsContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "Reservations";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
    // ReturnUrlParameter requires 
    //using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    string email = "tijanah@admin.com";
    string password = "Test1234*";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new User
        {
            FirstName = "Tijana",
            LastName = "Hristova",
            Email = email,
            PasswordHash = password,
            UserName = "Tici",
        };

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
        
}

app.Run();