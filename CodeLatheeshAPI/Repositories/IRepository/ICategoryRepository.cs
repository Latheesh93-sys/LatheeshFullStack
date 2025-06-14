using CodeLatheeshAPI.Models.DomainModels;

namespace CodeLatheeshAPI.Repositories.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync(int userId);

        Task<Category> FindByIdAsync(Guid id);  

        Task<Category?> UpdateCategoryById(Category category);

        Task<Category?> DeleteAsync(Guid id);

        Task<UserSummary> GetUserSummaryAsync(int userId);
    }
}
