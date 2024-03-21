using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReservations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyReservations.Services;
using MyReservations.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyReservations.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ReservationsContext _context;
        private readonly IFileUploadService _fileUpload;
     

        public RestaurantController(ReservationsContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUpload = fileUploadService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> Index()
        {
            var restaurants = await _context.Restaurants.ToListAsync();
            return View(restaurants);
        }

        [HttpGet]
        public async Task<ActionResult<Restaurant>> Detail(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant, IFormFile RestaurantImages)
        {
            if (ModelState.IsValid)
            {
                restaurant.RestaurantImages = _fileUpload.SaveImage(RestaurantImages, string.Empty);
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Restaurant restaurant, IFormFile restaurantPictures)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingRestaurant = await _context.Restaurants.FindAsync(id);

                    if (existingRestaurant == null)
                        return NotFound();

                    // Update the restaurant name
                    existingRestaurant.RestaurantName = restaurant.RestaurantName;

                    // Check if a new image is uploaded and it's different from the existing one
                    existingRestaurant.RestaurantImages = _fileUpload.SaveImage(restaurantPictures, existingRestaurant.RestaurantImages);

                    _context.Update(existingRestaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.RestaurantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(restaurant);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditDetails(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
                return NotFound();

            return View(restaurant);
        }

   [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetails(int id, Restaurant restaurant, IFormFile restaurantImages)
        {
            ModelState.Remove("RestaurantImages");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRestaurant = await _context.Restaurants.FindAsync(id);

                    existingRestaurant.RestaurantName = restaurant.RestaurantName;
                    existingRestaurant.RestaurantDesc = restaurant.RestaurantDesc;
                    existingRestaurant.RestaurantHours = restaurant.RestaurantHours;
                    existingRestaurant.RestaurantLocation = restaurant.RestaurantLocation;

                    if (restaurantImages != null)
                    {
                        existingRestaurant.RestaurantImages = _fileUpload.SaveImage(restaurantImages, existingRestaurant.RestaurantImages);
                    }

                    _context.Update(existingRestaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.RestaurantId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(restaurant);
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.RestaurantId == id);
        }
    }
}
