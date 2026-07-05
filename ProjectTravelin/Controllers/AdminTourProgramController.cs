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

        public async Task<IActionResult> AdminTourProgramList()
        {
            var values = await _tourProgramService.GetAllTourProgramAsync();
            var tours = await _tourService.GetAllTourAsync();

            ViewBag.Tours = tours.ToDictionary(
                x => x.TourId,
                x => $"{x.Title} - {x.City} / {x.Country}"
            );

            return View(values);
        }

        public async Task<IActionResult> CreateTourProgram()
        {
            var tours = await _tourService.GetAllTourAsync();
            ViewBag.Tours = tours;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTourProgram(CreateTourProgramDto createTourProgramDto)
        {
            await _tourProgramService.CreateTourProgramAsync(createTourProgramDto);
            return RedirectToAction("TourProgramList");
        }

        public async Task<IActionResult> DeleteTourProgram(string id)
        {
            await _tourProgramService.DeleteTourProgramAsync(id);
            return RedirectToAction("TourProgramList");
        }

        public async Task<IActionResult> UpdateTourProgram(string id)
        {
            var value = await _tourProgramService.GetTourProgramByIdAsync(id);

            var updateDto = new UpdateTourProgramDto
            {
                TourProgramId = value.TourProgramId,
                TourId = value.TourId,
                DayNumber = value.DayNumber,
                Title = value.Title,
                Description = value.Description,
                ImageUrl = value.ImageUrl
            };

            var tours = await _tourService.GetAllTourAsync();
            ViewBag.Tours = tours;

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTourProgram(UpdateTourProgramDto updateTourProgramDto)
        {
            await _tourProgramService.UpdateTourProgramAsync(updateTourProgramDto);
            return RedirectToAction("TourProgramList");
        }
    }
}