using AutoMapper;
using MongoDB.Driver;
using ProjectTravelin.Dtos.TourProgramDtos;
using ProjectTravelin.Entities;
using ProjectTravelin.Settings;

namespace ProjectTravelin.Services.TourProgramServices
{
    public class TourProgramService : ITourProgramService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<TourProgram> _tourProgramCollection;

        public TourProgramService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _tourProgramCollection = database.GetCollection<TourProgram>(databaseSettings.TourProgramCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultTourProgramDto>> GetAllTourProgramAsync()
        {
            var values = await _tourProgramCollection
                .Find(x => true)
                .SortBy(x => x.DayNumber)
                .ToListAsync();

            return _mapper.Map<List<ResultTourProgramDto>>(values);
        }

        public async Task<List<ResultTourProgramDto>> GetTourProgramByTourIdAsync(string tourId)
        {
            var values = await _tourProgramCollection
                .Find(x => x.TourId == tourId)
                .SortBy(x => x.DayNumber)
                .ToListAsync();

            return _mapper.Map<List<ResultTourProgramDto>>(values);
        }

        public async Task CreateTourProgramAsync(CreateTourProgramDto createTourProgramDto)
        {
            var value = _mapper.Map<TourProgram>(createTourProgramDto);
            await _tourProgramCollection.InsertOneAsync(value);
        }

        public async Task UpdateTourProgramAsync(UpdateTourProgramDto updateTourProgramDto)
        {
            var value = _mapper.Map<TourProgram>(updateTourProgramDto);

            await _tourProgramCollection.FindOneAndReplaceAsync(
                x => x.TourProgramId == updateTourProgramDto.TourProgramId,
                value
            );
        }

        public async Task DeleteTourProgramAsync(string id)
        {
            await _tourProgramCollection.DeleteOneAsync(x => x.TourProgramId == id);
        }

        public async Task<GetTourProgramByIdDto> GetTourProgramByIdAsync(string id)
        {
            var value = await _tourProgramCollection
                .Find(x => x.TourProgramId == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetTourProgramByIdDto>(value);
        }
    }
}