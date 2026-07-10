using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Services.CommentServices;
using ProjectTravelin.Services.TourServices;

namespace ProjectTravelin.Controllers
{
    public class AdminCommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITourService _tourService;

        public AdminCommentController(
            ICommentService commentService,
            ITourService tourService)
        {
            _commentService = commentService;
            _tourService = tourService;
        }

        public async Task<IActionResult> CommentList()
        {
            var values = await _commentService.GetAllCommentAsync();
            var tours = await _tourService.GetAllTourAsync();

            ViewBag.Tours = tours.ToDictionary(
                x => x.TourId,
                x => $"{x.Title} - {x.City} / {x.Country}"
            );

            ViewBag.TotalCommentCount = values.Count;
            ViewBag.ApprovedCommentCount = values.Count(x => x.IsStatus);
            ViewBag.PendingCommentCount = values.Count(x => !x.IsStatus);

            return View("~/Views/AdminComment/CommentList.cshtml", values);
        }

        public async Task<IActionResult> ApproveComment(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _commentService.ChangeCommentStatusAsync(id, true);
            }

            return RedirectToAction("CommentList");
        }

        public async Task<IActionResult> PassiveComment(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _commentService.ChangeCommentStatusAsync(id, false);
            }

            return RedirectToAction("CommentList");
        }

        public async Task<IActionResult> DeleteComment(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _commentService.DeleteCommentAsync(id);
            }

            return RedirectToAction("CommentList");
        }
    }
}