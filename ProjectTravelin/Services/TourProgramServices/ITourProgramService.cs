using ProjectTravelin.Dtos.TourProgramDtos;

namespace ProjectTravelin.Services.TourProgramServices
{
    public interface ITourProgramService
    {
        Task<List<ResultTourProgramDto>> GetAllTourProgramAsync();

        Task<List<ResultTourProgramDto>> GetTourProgramByTourIdAsync(string tourId);

        Task CreateTourProgramAsync(CreateTourProgramDto createTourProgramDto);

        Task UpdateTourProgramAsync(UpdateTourProgramDto updateTourProgramDto);

        Task DeleteTourProgramAsync(string id);

        Task<GetTourProgramByIdDto> GetTourProgramByIdAsync(string id);
    }
}
