using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.CategoryServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.ViewComponents
{
    [ViewComponent(Name = "_TourListComponentPartial")]
    public class _TourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;
        private readonly ICategoryService _categoryService;

        public _TourListComponentPartial(
            ITourService tourService,
            ICategoryService categoryService)
        {
            _tourService = tourService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1, string categoryId = "")
        {
            int pageSize = 4;

            if (page <= 0)
            {
                page = 1;
            }

            List<ResultTourDto> values;

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                values = await _tourService.GetToursByCategoryIdAsync(categoryId);
            }
            else
            {
                values = await _tourService.GetAllTourAsync();
            }

            values = values ?? new List<ResultTourDto>();

            var categories = await _categoryService.GetAllCategoryAsync();

            ViewBag.CategoryNames = categories.ToDictionary(
                x => x.CategoryId,
                x => x.CategoryName
            );

            int totalCount = values.Count;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            var pagedValues = values
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SelectedCategoryId = categoryId;

            return View(pagedValues);
        }
    }
}