using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public IActionResult CreateTour()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTour(CreateTourDto _createTourDto)
        {
            await _tourService.CreateTourAsync(_createTourDto);
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> TourList(string categoryId)
        {
            ViewBag.SelectedCategoryId = categoryId;

            List<ResultTourDto> values;

            if (!string.IsNullOrEmpty(categoryId))
            {
                values = await _tourService.GetToursByCategoryIdAsync(categoryId);
            }
            else
            {
                values = await _tourService.GetAllTourAsync();
            }

            return View(values);
        }

        public async Task<IActionResult> TourDetail(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }
    }
}