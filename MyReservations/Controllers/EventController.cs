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
    public class EventController : Controller
    {
        private readonly ReservationsContext _context;
        private readonly IFileUploadService _fileUpload;

     

        public EventController(ReservationsContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUpload = fileUploadService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Event>>> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        [HttpGet]
        public async Task<ActionResult<Event>> DetailE(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventItem, IFormFile EventPictures)
        {
            if (ModelState.IsValid)
            {
                eventItem.EventImages = _fileUpload.SaveImage(EventPictures, string.Empty);
                _context.Events.Add(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventItem, IFormFile EventPictures)
        {
          

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.Events.FindAsync(id);
                    if (existingEvent == null)
                    {
                        return NotFound();
                    }

                    existingEvent.EventName = eventItem.EventName;

                    if (EventPictures != null)
                    {
                        existingEvent.EventImages = _fileUpload.SaveImage(EventPictures, existingEvent.EventImages);
                    }

                    _context.Update(existingEvent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(eventItem);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> EditEDetails(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEDetails(int id, Event eventItem, IFormFile eventImages)
        {
            ModelState.Remove("EventImages");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.Events.FindAsync(id);

                    existingEvent.EventName = eventItem.EventName;
                    existingEvent.EventDesc = eventItem.EventDesc;
                    existingEvent.EventHours = eventItem.EventHours;
                    existingEvent.EventLocation = eventItem.EventLocation;

                  
                    if (eventImages != null)
                    {
                        existingEvent.EventImages = _fileUpload.SaveImage(eventImages, existingEvent.EventImages);
                    }

                    _context.Update(existingEvent);
                    await _context.SaveChangesAsync();
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.EventId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(eventItem);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
