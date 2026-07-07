using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.CategoryDtos;
using ProjectTravelin.Services.CategoryServices;

namespace ProjectTravelin.Controllers
{
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto _createCategoryDto)
        {
            _createCategoryDto.IsStatus = true;
            await _categoryService.CreateCategoryAsync(_createCategoryDto);
            return RedirectToAction("CategoryList");
        }
    }
}
