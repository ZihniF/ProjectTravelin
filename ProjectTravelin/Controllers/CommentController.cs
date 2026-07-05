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

        public CommentController(ICommentService commentService, ITourService tourService)
        {
            _commentService = commentService;
            _tourService = tourService;
        }

        public IActionResult CreateComment(string tourId)
        {
            var model = new CreateCommentDto
            {
                TourId = tourId
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

            createCommentDto.CommentDate = DateTime.Now;

            // Kullanıcı yorumu direkt listede görünsün diye true yaptık.
            // Admin onayı istersen bunu false yaparız.
            createCommentDto.IsStatus = true;

            await _commentService.CreateCommentAsync(createCommentDto);

            TempData["CommentSuccess"] = "Yorumunuz başarıyla gönderildi.";

            return RedirectToAction("CommentListByTourId", "Comment", new { id = createCommentDto.TourId });
        }

        public async Task<IActionResult> CommentListByTourId(string id)
        {
            var values = await _commentService.GetCommentsByTourId(id);
            var tour = await _tourService.GetTourByIdAsync(id);

            ViewBag.TourId = id;
            ViewBag.TourName = tour != null ? tour.Title : "Tur Yorumları";

            return View(values);
        }
    }
}