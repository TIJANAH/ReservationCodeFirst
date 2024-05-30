using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyReservations.Models;
using System.Linq;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly ReservationsContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(ReservationsContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [Authorize]
    public IActionResult Profile()
    {
        return View();
    }

    [Authorize]
    public IActionResult Edit(string id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Edit(string id, User user)
    {
        if (id != user.Id)
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

    [Authorize]
    public IActionResult Delete(string id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize]
    public IActionResult DeleteConfirmed(string id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
