using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.ViewComponents.TourViewComponents
{
    public class _TourListComponentPartial:ViewComponent
    {
        private readonly ITourService _tourService;

        public _TourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page =1)
        {
            int pageSize = 3;
            var values = await _tourService.GetAllTourAsync();
            var totalCount = values.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pageValues = values
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pageValues);
        }
    }
}
