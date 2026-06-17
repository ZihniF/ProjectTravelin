using Project3Travelin.Dtos.CommentDtos;

namespace Project3Travelin.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllCommentAsync();
        Task CreateCommentAsync(CreateCommentDto createCommentDto);
        Task UpdateCommentAsync(UpdateCommentDto updateCommentDto);
        Task DeleteCommentAsync(string id);
        Task<GetCommentByIdDto> GetCommentByIdAsync(string id);
    }
}
