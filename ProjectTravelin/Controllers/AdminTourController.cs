using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;

        public AdminTourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task< IActionResult> TourList()
        {
            var values = await _tourService.GetAllTourAsync();
            return View(values);
        }
        [HttpGet]
        public IActionResult CreateTour()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> CreateTour(CreateTourDto _createTourDto)
        {
            if (ModelState.IsValid)
            {
                await _tourService.CreateTourAsync(_createTourDto);
                return RedirectToAction("TourList");
            }
            return View(_createTourDto);
        }
    }
}
