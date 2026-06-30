using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Services.TourProgramServices;

namespace ProjectTravelin.ViewComponents.TourViewComponents
{
    public class _TourProgramComponentPartial : ViewComponent
    {
        private readonly ITourProgramService _tourProgramService;

        public _TourProgramComponentPartial(ITourProgramService tourProgramService)
        {
            _tourProgramService = tourProgramService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string tourId)
        {
            var values = await _tourProgramService.GetTourProgramByTourIdAsync(tourId);
            return View(values);
        }
    }
}