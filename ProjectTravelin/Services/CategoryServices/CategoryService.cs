using AutoMapper;
using MongoDB.Driver;
using ProjectTravelin.Dtos.CategoryDtos;
using ProjectTravelin.Entities;
using ProjectTravelin.Settings;

namespace ProjectTravelin.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);

            _mapper = mapper;
        }

        public async Task<List<ResultCategoryDto>> GetActiveCategoriesAsync()
        {
            var values = await _categoryCollection
                .Find(x => x.IsStatus == true)
                .SortBy(x => x.CategoryName)
                .ToListAsync();

            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var values = await _categoryCollection
                .Find(x => true)
                .SortBy(x => x.CategoryName)
                .ToListAsync();

            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var value = _mapper.Map<Category>(createCategoryDto);

            value.CategoryName = value.CategoryName ?? "";
            value.IconUrl = value.IconUrl ?? "";

            await _categoryCollection.InsertOneAsync(value);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var value = _mapper.Map<Category>(updateCategoryDto);

            value.CategoryName = value.CategoryName ?? "";
            value.IconUrl = value.IconUrl ?? "";

            await _categoryCollection.FindOneAndReplaceAsync(
                x => x.CategoryId == updateCategoryDto.CategoryId,
                value
            );
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryCollection.DeleteOneAsync(x => x.CategoryId == id);
        }

        public async Task<GetCategoryByIdDto> GetCategoryByIdAsync(string id)
        {
            var value = await _categoryCollection
                .Find(x => x.CategoryId == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetCategoryByIdDto>(value);
        }
    }
}