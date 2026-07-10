using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.CommentDtos;
using ProjectTravelin.Services.CommentServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITourService _tourService;

        public CommentController(
            ICommentService commentService,
            ITourService tourService)
        {
            _commentService = commentService;
            _tourService = tourService;
        }

        public async Task<IActionResult> CreateComment(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return RedirectToAction("TourList", "Tour");
            }

            var tour = await _tourService.GetTourByIdAsync(tourId);

            if (tour == null)
            {
                return NotFound();
            }

            ViewBag.TourName = tour.Title;

            var model = new CreateCommentDto
            {
                TourId = tourId,
                Headline = "",
                CommentDetail = "",
                Score = 5
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            if (string.IsNullOrEmpty(createCommentDto.TourId))
            {
                return RedirectToAction("TourList", "Tour");
            }

            if (string.IsNullOrWhiteSpace(createCommentDto.Headline))
            {
                ModelState.AddModelError("Headline", "Yorum başlığı boş bırakılamaz.");
            }

            if (string.IsNullOrWhiteSpace(createCommentDto.CommentDetail))
            {
                ModelState.AddModelError("CommentDetail", "Yorum detayı boş bırakılamaz.");
            }

            if (createCommentDto.Score < 1 || createCommentDto.Score > 5)
            {
                ModelState.AddModelError("Score", "Puan 1 ile 5 arasında olmalıdır.");
            }

            if (!ModelState.IsValid)
            {
                var tour = await _tourService.GetTourByIdAsync(createCommentDto.TourId);
                ViewBag.TourName = tour != null ? tour.Title : "Tur Yorumu";

                return View(createCommentDto);
            }

            await _commentService.CreateCommentAsync(createCommentDto);

            TempData["CommentSuccess"] = "Yorumunuz başarıyla gönderildi. Admin onayından sonra yayınlanacaktır.";

            return RedirectToAction("CommentListByTourId", "Comment", new { id = createCommentDto.TourId });
        }

        public async Task<IActionResult> CommentListByTourId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("TourList", "Tour");
            }

            var tour = await _tourService.GetTourByIdAsync(id);

            if (tour == null)
            {
                return NotFound();
            }

            var values = await _commentService.GetApprovedCommentsByTourIdAsync(id);

            ViewBag.TourId = id;
            ViewBag.TourName = tour.Title;

            return View(values);
        }
    }
}