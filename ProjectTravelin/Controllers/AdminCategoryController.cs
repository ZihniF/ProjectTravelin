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

        public async Task<IActionResult> CategoryList()
        {
            var values = await _categoryService.GetAllCategoryAsync();

            ViewBag.TotalCategoryCount = values.Count;
            ViewBag.ActiveCategoryCount = values.Count(x => x.IsStatus);
            ViewBag.PassiveCategoryCount = values.Count(x => !x.IsStatus);

            return View("~/Views/AdminCategory/CategoryList.cshtml", values);
        }

        public IActionResult CreateCategory()
        {
            var model = new CreateCategoryDto
            {
                CategoryName = "",
                IconUrl = "",
                IsStatus = true
            };

            return View("~/Views/AdminCategory/CreateCategory.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.CategoryName = createCategoryDto.CategoryName ?? "";
            createCategoryDto.IconUrl = createCategoryDto.IconUrl ?? "";

            if (string.IsNullOrWhiteSpace(createCategoryDto.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Kategori adı boş bırakılamaz.");

                return View("~/Views/AdminCategory/CreateCategory.cshtml", createCategoryDto);
            }

            createCategoryDto.IsStatus = true;

            await _categoryService.CreateCategoryAsync(createCategoryDto);

            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> UpdateCategory(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("CategoryList");
            }

            var value = await _categoryService.GetCategoryByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var model = new UpdateCategoryDto
            {
                CategoryId = value.CategoryId,
                CategoryName = value.CategoryName ?? "",
                IconUrl = value.IconUrl ?? "",
                IsStatus = value.IsStatus
            };

            return View("~/Views/AdminCategory/UpdateCategory.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            updateCategoryDto.CategoryName = updateCategoryDto.CategoryName ?? "";
            updateCategoryDto.IconUrl = updateCategoryDto.IconUrl ?? "";

            if (string.IsNullOrWhiteSpace(updateCategoryDto.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Kategori adı boş bırakılamaz.");

                return View("~/Views/AdminCategory/UpdateCategory.cshtml", updateCategoryDto);
            }

            await _categoryService.UpdateCategoryAsync(updateCategoryDto);

            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _categoryService.DeleteCategoryAsync(id);
            }

            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> MakeCategoryActive(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _categoryService.ChangeCategoryStatusAsync(id, true);
            }

            return RedirectToAction("CategoryList");
        }

        public async Task<IActionResult> MakeCategoryPassive(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _categoryService.ChangeCategoryStatusAsync(id, false);
            }

            return RedirectToAction("CategoryList");
        }
    }
}