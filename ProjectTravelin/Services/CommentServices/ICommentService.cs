using ProjectTravelin.Dtos.CommentDtos;

namespace ProjectTravelin.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDto>> GetAllCommentAsync();
        Task CreateCommentAsync(CreateCommentDto createCommentDto);
        Task UpdateCommentAsync(UpdateCommentDto updateCommentDto);
        Task DeleteCommentAsync(string id);
        Task<GetCommentByIdDto> GetCommentByIdAsync(string id);
        Task<List<ResultCommentListByTourIdDto>> GetCommentsByTourId(string id);

    }
}
