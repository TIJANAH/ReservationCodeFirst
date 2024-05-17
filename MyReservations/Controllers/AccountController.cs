using Microsoft.AspNetCore.Mvc;
using MyReservations.Models;
using System.Linq;
using BCrypt.Net;
using MyReservations.Services;

public class AccountController : Controller
{
    private readonly ReservationsContext _context;
    private readonly TokenService _tokenService;
    public AccountController(ReservationsContext context, TokenService token)
    {
        _context = context;
        _tokenService = token;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            var userToken = _tokenService.GenerateJwtToken(user);

            user.Token = userToken;
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ModelState.AddModelError(nameof(LoginViewModel.Username), "Invalid username or password");
            return View();
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Password = hashedPassword
            };

            _context.Users.Add(user);
            _context.SaveChanges();

           
            return RedirectToAction("Login");
        }

        return View(model);
    }

    public IActionResult Edit(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    public IActionResult Edit(int id, User user)
    {
        if (id != user.UserId)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            _context.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        return View(user);
    }

    public IActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Profile()
    {
      
        return View();
    }
}
