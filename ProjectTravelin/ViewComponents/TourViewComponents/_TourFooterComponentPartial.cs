using Microsoft.AspNetCore.Mvc;

namespace ProjectTravelin.ViewComponents.TourViewComponents
{
    public class _TourFooterComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
