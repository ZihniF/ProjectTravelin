using Project3Travelin.Dtos.CategoryDtos;

namespace Project3Travelin.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(string id);
        Task<GetCategoryByIdDto> GetCategoryByIdAsync(string id);


    }
}
