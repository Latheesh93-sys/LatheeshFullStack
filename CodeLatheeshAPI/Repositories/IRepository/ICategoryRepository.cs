using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;

namespace CodeLatheeshAPI.Repositories.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<Category> FindByIdAsync(Guid id);  

        Task<Category?> UpdateCategoryById(Category category);

        Task<Category?> DeleteAsync(Guid id);

        Task<UserSummary> GetUserSummaryAsync(int userId,int selectedmonth);

        Task<PaginatedResult<Category>> GetFilteredAsync(
        int userId,
        int month,
        string type,
        string paymentMethod,
        string sortBy,
        string sortOrder,
        int pageNumber = 1,
        int pageSize = 10);
    }
}
