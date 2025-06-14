using CodeLatheeshAPI.Models.DomainModels;

namespace CodeLatheeshAPI.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(Category category);

        Task<IEnumerable<Category>> GetAllCategories(int userId);

        Task<Category> FindCategory(Guid id);

        Task<Category?> UpdateCategoryById(Category category);

        Task<Category?> DeleteCategory(Guid id);
    }
}
