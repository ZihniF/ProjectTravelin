using ProjectTravelin.Dtos.BookingDtos;

namespace ProjectTravelin.Services.BookingServices
{
    public interface IBookingService
    {
        Task<List<ResultBookingDto>> GetAllBookingAsync();

        Task<List<ResultBookingDto>> GetBookingsByTourIdAsync(string tourId);

        Task CreateBookingAsync(CreateBookingDto createBookingDto);

        Task UpdateBookingAsync(UpdateBookingDto updateBookingDto);

        Task DeleteBookingAsync(string id);

        Task<GetBookingByIdDto> GetBookingByIdAsync(string id);

        Task ChangeBookingStatusAsync(string id, string status);
    }
}
