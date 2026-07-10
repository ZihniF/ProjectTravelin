using ProjectTravelin.Dtos.CategoryDtos;

namespace ProjectTravelin.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();

        Task<List<ResultCategoryDto>> GetActiveCategoriesAsync();

        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);

        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);

        Task DeleteCategoryAsync(string id);

        Task<GetCategoryByIdDto> GetCategoryByIdAsync(string id);

        Task ChangeCategoryStatusAsync(string id, bool status);
    }
}