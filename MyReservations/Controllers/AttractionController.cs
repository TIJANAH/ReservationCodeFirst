using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyReservations.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MyReservations.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyReservations.Interfaces;

namespace MyReservations.Controllers
{
    public class AttractionController : Controller
    {
        private readonly ReservationsContext _context;
        private readonly IFileUploadService _fileUpload;

        public AttractionController(ReservationsContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUpload = fileUploadService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Attraction>>> Index()
        {
            var attractions = await _context.Attractions.ToListAsync();
            return View(attractions);
        }

        [HttpGet]
        public async Task<ActionResult<Attraction>> DetailA(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null)
                return NotFound();

            return View(attraction);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attraction attraction, IFormFile AttractionsImages)
        {
            if (ModelState.IsValid)
            {
                attraction.AttractionsImages = _fileUpload.SaveImage(AttractionsImages, string.Empty);
                _context.Attractions.Add(attraction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attraction);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null)
                return NotFound();

            return View(attraction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Attraction attraction, IFormFile AttractionPictures)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingAttraction = await _context.Attractions.FindAsync(id);
                    if (existingAttraction == null)
                    {
                        return NotFound();
                    }

                    existingAttraction.AttractionsName = attraction.AttractionsName;

                    if (AttractionPictures != null)
                    {
                        existingAttraction.AttractionsImages = _fileUpload.SaveImage(AttractionPictures, existingAttraction.AttractionsImages);
                    }

                    _context.Update(existingAttraction);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionExists(attraction.AttractionsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(attraction);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null)
            {
                return NotFound();
            }

            return View(attraction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null)
            {
                return NotFound();
            }

            _context.Attractions.Remove(attraction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditADetails(int id)
        {
            var attraction = await _context.Attractions.FindAsync(id);
            if (attraction == null)
                return NotFound();

            return View(attraction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditADetails(int id, Attraction attraction, IFormFile AttractionsImages)
        {
            ModelState.Remove("AttractionImages");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAttraction = await _context.Attractions.FindAsync(id);

                    existingAttraction.AttractionsName = attraction.AttractionsName;
                    existingAttraction.AttractionsDesc = attraction.AttractionsDesc;
                    existingAttraction.AttractionsHours = attraction.AttractionsHours;
                    existingAttraction.AttractionsLocation = attraction.AttractionsLocation;

                    if (AttractionsImages != null)
                    {
                        existingAttraction.AttractionsImages = _fileUpload.SaveImage(AttractionsImages, existingAttraction.AttractionsImages);
                    }

                    _context.Update(existingAttraction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionExists(attraction.AttractionsId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(attraction);
        }

        private bool AttractionExists(int id)
        {
            return _context.Attractions.Any(e => e.AttractionsId == id);
        }
    }
}
