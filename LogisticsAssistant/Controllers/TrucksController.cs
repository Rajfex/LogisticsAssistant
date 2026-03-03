using System.Security.Claims;
using LogisticsAssistant.Interfaces;
using LogisticsAssistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistant.Controllers
{
    public class TrucksController : Controller
    {
        private readonly ITrucksService _truckService;
        private readonly AppDbContext _context;

        public TrucksController(ITrucksService truckService, AppDbContext context)
        {
            _truckService = truckService;
            _context = context;

        }

        [Authorize]
        public IActionResult Index()
        {
            var trucks = _context.Trucks.ToList();
            return View(trucks);
        }

        // GET: /Trucks/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: /Trucks/Create
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(TruckViewModel truckView)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            await _truckService.CreateTruckAsync(truckView);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var truck = await _context.Trucks.FirstOrDefaultAsync(t => t.Id == id);
            if (truck == null)
            {
                return NotFound();
            }
            return View(truck);
        }

        // DELETE: /Trucks/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (_truckService.DeleteTruckAsync(id).Result)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
