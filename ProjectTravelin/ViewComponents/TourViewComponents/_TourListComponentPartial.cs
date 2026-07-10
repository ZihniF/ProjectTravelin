using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.ViewComponents
{
    [ViewComponent(Name = "_TourListComponentPartial")]
    public class _TourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;

        public _TourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
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