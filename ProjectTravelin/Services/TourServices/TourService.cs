using AutoMapper;
using MongoDB.Driver;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Entities;
using ProjectTravelin.Settings;

namespace ProjectTravelin.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;
        public TourService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
            var values = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(values);
            values.CategoryId = values.CategoryId ?? "";
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x => x.TourId == id);
        }

        public async Task<List<ResultTourDto>> GetAllTourAsync()
        {
            var values =await _tourCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>( values);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var value =await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(value);
        }

        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var values = _mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x => x.TourId == updateTourDto.TourId, values);
            values.CategoryId = values.CategoryId ?? "";
        }
        public async Task<List<ResultTourDto>> GetToursByPageAsync(int page, int pageSize)
        {
            var values = await _tourCollection
                .Find(x => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<long> GetTourCountAsync()
        {
            return await _tourCollection.CountDocumentsAsync(x => true);
        }
        public async Task<List<ResultTourDto>> GetToursByCategoryIdAsync(string categoryId)
        {
            var values = await _tourCollection
                .Find(x => x.CategoryId == categoryId)
                .SortByDescending(x => x.TourDate)
                .ToListAsync();

            return _mapper.Map<List<ResultTourDto>>(values);
        }
    }
}
