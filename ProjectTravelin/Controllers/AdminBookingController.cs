using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Services.BookingServices;
using ProjectTravelin.Services.EmailServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class AdminBookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ITourService _tourService;
        private readonly IEmailService _emailService;

        public AdminBookingController(
            IBookingService bookingService,
            ITourService tourService,
            IEmailService emailService)
        {
            _bookingService = bookingService;
            _tourService = tourService;
            _emailService = emailService;
        }

        public async Task<IActionResult> BookingList()
        {
            var values = await _bookingService.GetAllBookingAsync();
            var tours = await _tourService.GetAllTourAsync();

            ViewBag.Tours = tours.ToDictionary(
                x => x.TourId,
                x => $"{x.Title} - {x.City} / {x.Country}"
            );

            ViewBag.TotalBookingCount = values.Count;
            ViewBag.PendingBookingCount = values.Count(x => x.Status == "Beklemede");
            ViewBag.ApprovedBookingCount = values.Count(x => x.Status == "Onaylandı");
            ViewBag.CancelledBookingCount = values.Count(x => x.Status == "İptal Edildi");

            return View("~/Views/AdminBooking/BookingList.cshtml", values);
        }
        public async Task<IActionResult> TourBookingList(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("BookingList");
            }

            var values = await _bookingService.GetBookingsByTourIdAsync(tourId);
            var tour = await _tourService.GetTourByIdAsync(tourId);

            ViewBag.SelectedTourId = tourId;

            if (tour != null)
            {
                ViewBag.SelectedTourName = $"{tour.Title} - {tour.City} / {tour.Country}";
            }
            else
            {
                ViewBag.SelectedTourName = "Tur bilgisi bulunamadı";
            }

            ViewBag.TotalBookingCount = values.Count;
            ViewBag.PendingBookingCount = values.Count(x => x.Status == "Beklemede");
            ViewBag.ApprovedBookingCount = values.Count(x => x.Status == "Onaylandı");
            ViewBag.CancelledBookingCount = values.Count(x => x.Status == "İptal Edildi");

            return View("~/Views/AdminBooking/TourBookingList.cshtml", values);
        }

        public async Task<IActionResult> ApprovedBooking(string id, string tourId)
        {
            if (string.IsNullOrEmpty(id))
            {
                if (!string.IsNullOrEmpty(tourId))
                {
                    return RedirectToAction("TourBookingList", new { tourId = tourId });
                }

                return RedirectToAction("BookingList");
            }

            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                TempData["BookingStatusMessage"] = "Rezervasyon bulunamadı.";
                TempData["BookingStatusType"] = "error";

                if (!string.IsNullOrEmpty(tourId))
                {
                    return RedirectToAction("TourBookingList", new { tourId = tourId });
                }

                return RedirectToAction("BookingList");
            }

            var tours = await _tourService.GetAllTourAsync();

            var tour = tours.FirstOrDefault(x => x.TourId == booking.TourId);

            var tourName = tour != null
                ? $"{tour.Title} - {tour.City} / {tour.Country}"
                : "Tur bilgisi bulunamadı";

            await _bookingService.ChangeBookingStatusAsync(id, "Onaylandı");

            try
            {
                if (!string.IsNullOrWhiteSpace(booking.Email))
                {
                    await _emailService.SendBookingApprovedEmailAsync(
                        booking.Email,
                        booking.NameSurname,
                        tourName,
                        booking.BookingDate.ToString("dd.MM.yyyy")
                    );

                    TempData["BookingStatusMessage"] = "Rezervasyon onaylandı ve müşteriye onay e-postası gönderildi.";
                    TempData["BookingStatusType"] = "success";
                }
                else
                {
                    TempData["BookingStatusMessage"] = "Rezervasyon onaylandı fakat müşterinin e-posta adresi bulunamadı.";
                    TempData["BookingStatusType"] = "warning";
                }
            }
            catch
            {
                TempData["BookingStatusMessage"] = "Rezervasyon onaylandı fakat e-posta gönderilirken bir hata oluştu.";
                TempData["BookingStatusType"] = "warning";
            }

            if (!string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("TourBookingList", new { tourId = tourId });
            }

            return RedirectToAction("BookingList");
        }

        public async Task<IActionResult> PendingBooking(string id, string tourId)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _bookingService.ChangeBookingStatusAsync(id, "Beklemede");

                TempData["BookingStatusMessage"] = "Rezervasyon tekrar beklemeye alındı.";
                TempData["BookingStatusType"] = "warning";
            }

            if (!string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("TourBookingList", new { tourId = tourId });
            }

            return RedirectToAction("BookingList");
        }

        public async Task<IActionResult> CancelBooking(string id, string tourId)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _bookingService.ChangeBookingStatusAsync(id, "İptal Edildi");

                TempData["BookingStatusMessage"] = "Rezervasyon iptal edildi.";
                TempData["BookingStatusType"] = "error";
            }

            if (!string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("TourBookingList", new { tourId = tourId });
            }

            return RedirectToAction("BookingList");
        }

        public async Task<IActionResult> DeleteBooking(string id, string tourId)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _bookingService.DeleteBookingAsync(id);

                TempData["BookingStatusMessage"] = "Rezervasyon silindi.";
                TempData["BookingStatusType"] = "error";
            }

            if (!string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("TourBookingList", new { tourId = tourId });
            }

            return RedirectToAction("BookingList");
        }
    }
}