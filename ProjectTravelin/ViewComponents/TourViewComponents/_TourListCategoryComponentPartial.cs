using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Services.CategoryServices;

namespace ProjectTravelin.ViewComponents
{
    public class _TourListCategoryComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _TourListCategoryComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.GetActiveCategoriesAsync();

            return View(values);
        }
    }
}