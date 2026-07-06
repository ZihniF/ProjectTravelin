using AutoMapper;
using MongoDB.Driver;
using ProjectTravelin.Dtos.BookingDtos;
using ProjectTravelin.Entities;
using ProjectTravelin.Settings;

namespace ProjectTravelin.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Booking> _bookingCollection;

        public BookingService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _bookingCollection = database.GetCollection<Booking>(databaseSettings.BookingCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultBookingDto>> GetAllBookingAsync()
        {
            var values = await _bookingCollection
                .Find(x => true)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultBookingDto>>(values);
        }

        public async Task<List<ResultBookingDto>> GetBookingsByTourIdAsync(string tourId)
        {
            var values = await _bookingCollection
                .Find(x => x.TourId == tourId)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultBookingDto>>(values);
        }

        public async Task CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            var value = _mapper.Map<Booking>(createBookingDto);

            if (string.IsNullOrEmpty(value.Status))
            {
                value.Status = "Beklemede";
            }

            if (value.CreatedDate == default)
            {
                value.CreatedDate = DateTime.Now;
            }

            await _bookingCollection.InsertOneAsync(value);
        }

        public async Task UpdateBookingAsync(UpdateBookingDto updateBookingDto)
        {
            var value = _mapper.Map<Booking>(updateBookingDto);

            await _bookingCollection.FindOneAndReplaceAsync(
                x => x.BookingId == updateBookingDto.BookingId,
                value
            );
        }

        public async Task DeleteBookingAsync(string id)
        {
            await _bookingCollection.DeleteOneAsync(x => x.BookingId == id);
        }

        public async Task<GetBookingByIdDto> GetBookingByIdAsync(string id)
        {
            var value = await _bookingCollection
                .Find(x => x.BookingId == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetBookingByIdDto>(value);
        }

        public async Task ChangeBookingStatusAsync(string id, string status)
        {
            var filter = Builders<Booking>.Filter.Eq(x => x.BookingId, id);

            var update = Builders<Booking>.Update
                .Set(x => x.Status, status);

            await _bookingCollection.UpdateOneAsync(filter, update);
        }
    }
}