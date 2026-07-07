using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using ProjectTravelin.Services.BookingServices;
using ProjectTravelin.Services.EmailServices;
using ProjectTravelin.Services.TourServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
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

        public async Task<IActionResult> ExportTourBookingsToExcel(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("BookingList");
            }

            var bookings = await _bookingService.GetBookingsByTourIdAsync(tourId);
            var tour = await _tourService.GetTourByIdAsync(tourId);

            var tourName = tour != null
                ? $"{tour.Title} - {tour.City} / {tour.Country}"
                : "Tur Bilgisi Bulunamadı";

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Tur Rezervasyonları");

            worksheet.Cell(1, 1).Value = "Tur Adı";
            worksheet.Cell(1, 2).Value = tourName;

            worksheet.Cell(2, 1).Value = "Oluşturulma Tarihi";
            worksheet.Cell(2, 2).Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            worksheet.Cell(4, 1).Value = "Ad Soyad";
            worksheet.Cell(4, 2).Value = "E-Posta";
            worksheet.Cell(4, 3).Value = "Telefon";
            worksheet.Cell(4, 4).Value = "Kişi Sayısı";
            worksheet.Cell(4, 5).Value = "Tur Tarihi";
            worksheet.Cell(4, 6).Value = "Talep Tarihi";
            worksheet.Cell(4, 7).Value = "Durum";
            worksheet.Cell(4, 8).Value = "Not";

            var headerRange = worksheet.Range(4, 1, 4, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 5;

            foreach (var item in bookings)
            {
                worksheet.Cell(row, 1).Value = item.NameSurname;
                worksheet.Cell(row, 2).Value = item.Email;
                worksheet.Cell(row, 3).Value = item.Phone;
                worksheet.Cell(row, 4).Value = item.PersonCount;
                worksheet.Cell(row, 5).Value = item.BookingDate.ToString("dd.MM.yyyy");
                worksheet.Cell(row, 6).Value = item.CreatedDate.ToString("dd.MM.yyyy HH:mm");
                worksheet.Cell(row, 7).Value = item.Status;
                worksheet.Cell(row, 8).Value = item.Note;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var content = stream.ToArray();

            var fileName = $"Tur-Rezervasyonlari-{DateTime.Now:yyyyMMddHHmm}.xlsx";

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }
        public async Task<IActionResult> ExportTourBookingsToPdf(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("BookingList");
            }

            var bookings = await _bookingService.GetBookingsByTourIdAsync(tourId);
            var tour = await _tourService.GetTourByIdAsync(tourId);

            var tourName = tour != null
                ? $"{tour.Title} - {tour.City} / {tour.Country}"
                : "Tur Bilgisi Bulunamadı";

            var reportDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(25);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily("Arial"));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("Tur Rezervasyon Raporu")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken3);

                        column.Item().PaddingTop(5).Text($"Tur: {tourName}")
                            .FontSize(11)
                            .SemiBold();

                        column.Item().Text($"Rapor Tarihi: {reportDate}")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);

                        column.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().PaddingTop(15).Column(column =>
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Background(Colors.Grey.Lighten3).Padding(10).Column(c =>
                            {
                                c.Item().Text("Toplam Rezervasyon").FontSize(8).FontColor(Colors.Grey.Darken1);
                                c.Item().Text(bookings.Count.ToString()).FontSize(16).Bold();
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Yellow.Lighten4).Padding(10).Column(c =>
                            {
                                c.Item().Text("Beklemede").FontSize(8).FontColor(Colors.Grey.Darken1);
                                c.Item().Text(bookings.Count(x => x.Status == "Beklemede").ToString()).FontSize(16).Bold();
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Green.Lighten4).Padding(10).Column(c =>
                            {
                                c.Item().Text("Onaylandı").FontSize(8).FontColor(Colors.Grey.Darken1);
                                c.Item().Text(bookings.Count(x => x.Status == "Onaylandı").ToString()).FontSize(16).Bold();
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Background(Colors.Red.Lighten4).Padding(10).Column(c =>
                            {
                                c.Item().Text("İptal Edildi").FontSize(8).FontColor(Colors.Grey.Darken1);
                                c.Item().Text(bookings.Count(x => x.Status == "İptal Edildi").ToString()).FontSize(16).Bold();
                            });
                        });

                        column.Item().PaddingTop(18).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);   // Ad Soyad
                                columns.RelativeColumn(2);   // E-Posta
                                columns.RelativeColumn(1.5f); // Telefon
                                columns.RelativeColumn(1);   // Kişi
                                columns.RelativeColumn(1.3f); // Tur Tarihi
                                columns.RelativeColumn(1.5f); // Talep Tarihi
                                columns.RelativeColumn(1.2f); // Durum
                                columns.RelativeColumn(2);   // Not
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Ad Soyad");
                                header.Cell().Element(HeaderCell).Text("E-Posta");
                                header.Cell().Element(HeaderCell).Text("Telefon");
                                header.Cell().Element(HeaderCell).Text("Kişi");
                                header.Cell().Element(HeaderCell).Text("Tur Tarihi");
                                header.Cell().Element(HeaderCell).Text("Talep Tarihi");
                                header.Cell().Element(HeaderCell).Text("Durum");
                                header.Cell().Element(HeaderCell).Text("Not");
                            });

                            foreach (var item in bookings)
                            {
                                table.Cell().Element(BodyCell).Text(item.NameSurname ?? "-");
                                table.Cell().Element(BodyCell).Text(item.Email ?? "-");
                                table.Cell().Element(BodyCell).Text(item.Phone ?? "-");
                                table.Cell().Element(BodyCell).Text($"{item.PersonCount} kişi");
                                table.Cell().Element(BodyCell).Text(item.BookingDate.ToString("dd.MM.yyyy"));
                                table.Cell().Element(BodyCell).Text(item.CreatedDate.ToString("dd.MM.yyyy HH:mm"));
                                table.Cell().Element(BodyCell).Text(item.Status ?? "-");
                                table.Cell().Element(BodyCell).Text(string.IsNullOrWhiteSpace(item.Note) ? "-" : item.Note);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("ProjectTravelin - ");
                        text.Span("Rezervasyon Raporu").SemiBold();
                    });
                });
            }).GeneratePdf();

            var fileName = $"Tur-Rezervasyonlari-{DateTime.Now:yyyyMMddHHmm}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        private static IContainer HeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Blue.Darken3)
                .PaddingVertical(6)
                .PaddingHorizontal(5)
                .Border(1)
                .BorderColor(Colors.White)
                .DefaultTextStyle(x => x.FontColor(Colors.White).Bold().FontSize(8));
        }

        private static IContainer BodyCell(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(6)
                .PaddingHorizontal(5)
                .DefaultTextStyle(x => x.FontSize(8));
        }
    }
}