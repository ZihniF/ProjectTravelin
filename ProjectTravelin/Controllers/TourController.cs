using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.CategoryServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;

        public TourController(
            ITourService tourService,
            ICategoryService categoryService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
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

            ViewBag.CategoryName = "Kategori Yok";

            if (!string.IsNullOrWhiteSpace(value.CategoryId))
            {
                var categories = await _categoryService.GetAllCategoryAsync();

                var category = categories.FirstOrDefault(x => x.CategoryId == value.CategoryId);

                if (category != null)
                {
                    ViewBag.CategoryName = category.CategoryName;
                }
            }

            return View(value);
        }
    }
}