using AutoMapper;
using MongoDB.Driver;
using Project3Travelin.Dtos.CategoryDtos;
using Project3Travelin.Entities;
using Project3Travelin.Settings;

namespace Project3Travelin.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            var client=new MongoClient(_databaseSettings.ConnectionString);
            var database= client.GetDatabase(_databaseSettings.DatabaseName);
            _categoryCollection=database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var values = _mapper.Map<Category>(createCategoryDto);
            await _categoryCollection.InsertOneAsync(values);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryCollection.DeleteOneAsync(x=> x.CategoryId==id);
        }

        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var values=await _categoryCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCategoryDto>>(values);
        }

        public async Task<GetCategoryByIdDto> GetCategoryByIdAsync(string id)
        {
            var value =await _categoryCollection.Find(x => x.CategoryId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetCategoryByIdDto>(value);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var values = _mapper.Map<Category>(updateCategoryDto);
            await _categoryCollection.FindOneAndReplaceAsync(x=> x.CategoryId==updateCategoryDto.CategoryId, values);
        }
    }
}
