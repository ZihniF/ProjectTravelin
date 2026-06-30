using Microsoft.AspNetCore.Mvc;
using ProjectTravelin.Dtos.CommentDtos;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Services.CommentServices;

namespace ProjectTravelin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            createCommentDto.CommentDate = DateTime.Now;
            createCommentDto.IsStatus = false;
            await _commentService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("CommentList");
        }
    }
}
