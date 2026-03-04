using LogisticsAssistant.Interfaces;
using LogisticsAssistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistant.Controllers
{
    public class TruckRouteController : Controller
    {
        private readonly ITruckRouteService _truckRouteService;

        public TruckRouteController(ITruckRouteService truckRouteService)
        {
            _truckRouteService = truckRouteService;
        }

        public async Task<IActionResult> Index()
        {
            var routes = await _truckRouteService.GetAllTruckRoutesAsync();
            ViewBag.Routes = routes;
            return View();
        }
        [Authorize]
        public IActionResult Create()
        {
            var trucks = _truckRouteService.GetAllTrucks().Result;

            ViewBag.Trucks = trucks.Select(t => new SelectListItem
            {
                Value = t.Key.ToString(),
                Text = t.Value
            }).ToList();

            return View();
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create(TruckRouteViewModel truckRouteView)
        {
            if (!ModelState.IsValid)
            {
                var trucks = await _truckRouteService.GetAllTrucks();
                ViewBag.Trucks = trucks.Select(t => new SelectListItem
                {
                    Value = t.Key.ToString(),
                    Text = t.Value
                }).ToList();

                return View(truckRouteView);
            }

            var result = await _truckRouteService.CreateTruckRouteAsync(truckRouteView);
            if (!result)
            {
                var trucks = await _truckRouteService.GetAllTrucks();
                ViewBag.Trucks = trucks.Select(t => new SelectListItem
                {
                    Value = t.Key.ToString(),
                    Text = t.Value
                }).ToList();
                return View(truckRouteView);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RouteDetail(int id)
        {
            var route = await _truckRouteService.GetTruckRouteByIdAsync(id);

            if(route == null) {
                return NotFound();
            }

            return View(route);
        }

        public async Task<IActionResult> GanttChart(int id)
        {
            var routes = await _truckRouteService.GetAllTruckRoutesAsync();
            return View(routes);
        }

    }
}
