using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.BookingDtos;
using ProjectTravelin.Services.BookingServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITourService _tourService;

        public BookingController(IBookingService bookingService, ITourService tourService)
        {
            _bookingService = bookingService;
            _tourService = tourService;
        }

        public async Task<IActionResult> CreateBooking(string id)
        {
            var tours = await _tourService.GetAllTourAsync();
            ViewBag.Tours = tours;

            if (!string.IsNullOrEmpty(id))
            {
                var tour = await _tourService.GetTourByIdAsync(id);

                if (tour == null)
                {
                    return RedirectToAction("TourList", "Tour");
                }

                ViewBag.Tour = tour;

                var selectedModel = new CreateBookingDto
                {
                    TourId = id,
                    PersonCount = 1,
                    BookingDate = tour.TourDate,
                    Status = "Beklemede",
                    CreatedDate = DateTime.Now,
                    NameSurname = "",
                    Email = "",
                    Phone = "",
                    Note = ""
                };

                return View("~/Views/Booking/CreateBooking.cshtml", selectedModel);
            }

            ViewBag.Tour = null;

            var model = new CreateBookingDto
            {
                TourId = "",
                PersonCount = 1,
                BookingDate = DateTime.Now,
                Status = "Beklemede",
                CreatedDate = DateTime.Now,
                NameSurname = "",
                Email = "",
                Phone = "",
                Note = ""
            };

            return View("~/Views/Booking/CreateBooking.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(CreateBookingDto createBookingDto)
        {
            if (string.IsNullOrEmpty(createBookingDto.TourId))
            {
                var tours = await _tourService.GetAllTourAsync();

                ViewBag.Tours = tours;
                ViewBag.Tour = null;

                ModelState.AddModelError("TourId", "Lütfen rezervasyon yapmak istediğiniz turu seçiniz.");

                return View("~/Views/Booking/CreateBooking.cshtml", createBookingDto);
            }

            var selectedTour = await _tourService.GetTourByIdAsync(createBookingDto.TourId);

            if (selectedTour == null)
            {
                return RedirectToAction("TourList", "Tour");
            }

            if (createBookingDto.PersonCount <= 0)
            {
                createBookingDto.PersonCount = 1;
            }

            createBookingDto.NameSurname = createBookingDto.NameSurname ?? "";
            createBookingDto.Email = createBookingDto.Email ?? "";
            createBookingDto.Phone = createBookingDto.Phone ?? "";
            createBookingDto.Note = createBookingDto.Note ?? "";

            createBookingDto.BookingDate = selectedTour.TourDate;
            createBookingDto.Status = "Beklemede";
            createBookingDto.CreatedDate = DateTime.Now;

            await _bookingService.CreateBookingAsync(createBookingDto);

            TempData["BookingSuccess"] = "Rezervasyon talebiniz başarıyla alındı. En kısa sürede sizinle iletişime geçilecektir.";

            return RedirectToAction("BookingSuccess", "Booking");
        }

        public IActionResult BookingSuccess()
        {
            return View("~/Views/Booking/BookingSuccess.cshtml");
        }
    }
}