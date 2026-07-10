using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.CategoryServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class AdminTourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;

        public AdminTourController(ITourService tourService, ICategoryService categoryService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> TourList()
        {
            var values = await _tourService.GetAllTourAsync();
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTour()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Categories = categories;

            var model = new CreateTourDto
            {
                CategoryId = "",
                Title = "",
                Country = "",
                City = "",
                Description = "",
                DayNight = "",
                ImageUrl = "",
                GeminiImageUrl = "",
                YoutubeVideoUrl = "",
                TourDate = DateTime.Now,
                Capacity = 1
            };

            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> CreateTour(CreateTourDto _createTourDto)
        {
            await _tourService.CreateTourAsync(_createTourDto);
            if (string.IsNullOrEmpty(_createTourDto.CategoryId))
            {
                var categories = await _categoryService.GetActiveCategoriesAsync();
                ViewBag.Categories = categories;

                ModelState.AddModelError("CategoryId", "Lütfen bir kategori seçiniz.");

                return View(_createTourDto);
            }
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> DeleteTour(string id)
        {
            await _tourService.DeleteTourAsync(id);
            return RedirectToAction("TourList");
        }

        public async Task<IActionResult> UpdateTour(string id)
        {
            var value = await _tourService.GetTourByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Categories = categories;

            var model = new UpdateTourDto
            {
                TourId = value.TourId,
                CategoryId = value.CategoryId,
                Title = value.Title,
                Country = value.Country,
                City = value.City,
                Description = value.Description,
                Capacity = value.Capacity,
                TourDate = value.TourDate,
                DayNight = value.DayNight,
                ImageUrl = value.ImageUrl,
                GeminiImageUrl = value.GeminiImageUrl,
                YoutubeVideoUrl = value.YoutubeVideoUrl
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> UpdateTour(UpdateTourDto updateTourDto)
        {
            if (string.IsNullOrEmpty(updateTourDto.CategoryId))
            {
                var categories = await _categoryService.GetActiveCategoriesAsync();
                ViewBag.Categories = categories;

                ModelState.AddModelError("CategoryId", "Lütfen bir kategori seçiniz.");

                return View(updateTourDto);
            }
            await _tourService.UpdateTourAsync(updateTourDto);
            
            return RedirectToAction("TourList");
        }
    }
}
