using Microsoft.AspNetCore.Mvc;

namespace ProjectTravelin.ViewComponents.TourViewComponents
{
    public class _TourHeaderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
