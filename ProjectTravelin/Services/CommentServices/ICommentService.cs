using ProjectTravelin.Dtos.CommentDtos;

namespace ProjectTravelin.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllCommentAsync();

        Task<List<ResultCommentDto>> GetCommentsByTourIdAsync(string tourId);

        Task<List<ResultCommentDto>> GetApprovedCommentsByTourIdAsync(string tourId);

        Task CreateCommentAsync(CreateCommentDto createCommentDto);

        Task DeleteCommentAsync(string id);

        Task ChangeCommentStatusAsync(string id, bool status);

        Task<GetCommentByIdDto> GetCommentByIdAsync(string id);
    }
}