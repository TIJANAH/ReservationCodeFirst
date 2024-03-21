using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReservations.Models;

namespace MyReservations.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ReservationsContext _context;

        public ReservationController(ReservationsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _context.Restaurants.ToListAsync());
        }
    }
}
