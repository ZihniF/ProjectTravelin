using Microsoft.AspNetCore.Mvc;

namespace ProjectTravelin.ViewComponents.TourViewComponents
{
    public class _TourHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
