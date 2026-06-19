using Microsoft.AspNetCore.Mvc;

namespace ProjectTravelin.Controllers
{
    public class AdminTourController : Controller
    {
        public IActionResult TourList()
        {
            return View();
        }
    }
}
