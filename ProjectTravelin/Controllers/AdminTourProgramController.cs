using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.TourProgramDtos;
using ProjectTravelin.Services.TourProgramServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class AdminTourProgramController : Controller
    {
        private readonly ITourProgramService _tourProgramService;
        private readonly ITourService _tourService;

        public AdminTourProgramController(
            ITourProgramService tourProgramService,
            ITourService tourService)
        {
            _tourProgramService = tourProgramService;
            _tourService = tourService;
        }

        public async Task<IActionResult> AdminTourProgramList(string tourId)
        {
            var values = await _tourProgramService.GetAllTourProgramAsync();
            var tours = await _tourService.GetAllTourAsync();

            if (!string.IsNullOrEmpty(tourId))
            {
                values = values
                    .Where(x => x.TourId == tourId)
                    .OrderBy(x => x.DayNumber)
                    .ToList();
            }

            ViewBag.Tours = tours.ToDictionary(
                x => x.TourId,
                x => $"{x.Title} - {x.City} / {x.Country}"
            );

            ViewBag.SelectedTourId = tourId;

            if (!string.IsNullOrEmpty(tourId))
            {
                var selectedTour = tours.FirstOrDefault(x => x.TourId == tourId);

                if (selectedTour != null)
                {
                    ViewBag.SelectedTourName = $"{selectedTour.Title} - {selectedTour.City} / {selectedTour.Country}";
                }
            }

            return View("~/Views/AdminTourProgram/AdminTourProgramList.cshtml", values);
        }

        public async Task<IActionResult> CreateTourProgram(string tourId)
        {
            var tours = await _tourService.GetAllTourAsync();

            ViewBag.Tours = tours;
            ViewBag.SelectedTourId = tourId;

            var model = new CreateTourProgramDto
            {
                TourId = tourId,
                DayNumber = 1,
                Title = "",
                Description = "",
                ImageUrl = ""
            };

            return View("~/Views/AdminTourProgram/CreateTourProgram.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTourProgram(CreateTourProgramDto createTourProgramDto)
        {
            if (string.IsNullOrEmpty(createTourProgramDto.TourId))
            {
                var tours = await _tourService.GetAllTourAsync();

                ViewBag.Tours = tours;
                ViewBag.SelectedTourId = createTourProgramDto.TourId;

                ModelState.AddModelError("TourId", "Lütfen bir tur seçiniz.");

                return View("~/Views/AdminTourProgram/CreateTourProgram.cshtml", createTourProgramDto);
            }

            if (createTourProgramDto.DayNumber <= 0)
            {
                createTourProgramDto.DayNumber = 1;
            }

            createTourProgramDto.Title = createTourProgramDto.Title ?? "";
            createTourProgramDto.Description = createTourProgramDto.Description ?? "";
            createTourProgramDto.ImageUrl = createTourProgramDto.ImageUrl ?? "";

            await _tourProgramService.CreateTourProgramAsync(createTourProgramDto);

            return RedirectToAction("AdminTourProgramList", new { tourId = createTourProgramDto.TourId });
        }
        public async Task<IActionResult> DeleteTourProgram(string id, string tourId)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _tourProgramService.DeleteTourProgramAsync(id);
            }

            return RedirectToAction("AdminTourProgramList", new { tourId = tourId });
        }

        public async Task<IActionResult> UpdateTourProgram(string id)
        {
            var value = await _tourProgramService.GetTourProgramByIdAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateTourProgramDto
            {
                TourProgramId = value.TourProgramId,
                TourId = value.TourId,
                DayNumber = value.DayNumber,
                Title = value.Title ?? "",
                Description = value.Description ?? "",
                ImageUrl = value.ImageUrl ?? ""
            };

            var tours = await _tourService.GetAllTourAsync();

            ViewBag.Tours = tours;
            ViewBag.SelectedTourId = value.TourId;

            return View("~/Views/AdminTourProgram/UpdateTourProgram.cshtml", updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTourProgram(UpdateTourProgramDto updateTourProgramDto)
        {
            if (string.IsNullOrEmpty(updateTourProgramDto.TourProgramId))
            {
                return RedirectToAction("AdminTourProgramList", new { tourId = updateTourProgramDto.TourId });
            }

            if (string.IsNullOrEmpty(updateTourProgramDto.TourId))
            {
                var tours = await _tourService.GetAllTourAsync();

                ViewBag.Tours = tours;
                ViewBag.SelectedTourId = updateTourProgramDto.TourId;

                ModelState.AddModelError("TourId", "Lütfen bir tur seçiniz.");

                return View("~/Views/AdminTourProgram/UpdateTourProgram.cshtml", updateTourProgramDto);
            }

            if (updateTourProgramDto.DayNumber <= 0)
            {
                updateTourProgramDto.DayNumber = 1;
            }

            updateTourProgramDto.Title = updateTourProgramDto.Title ?? "";
            updateTourProgramDto.Description = updateTourProgramDto.Description ?? "";
            updateTourProgramDto.ImageUrl = updateTourProgramDto.ImageUrl ?? "";

            await _tourProgramService.UpdateTourProgramAsync(updateTourProgramDto);

            return RedirectToAction("AdminTourProgramList", new { tourId = updateTourProgramDto.TourId });
        }
    }
}