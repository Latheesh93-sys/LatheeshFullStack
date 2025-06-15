using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace CodeLatheeshAPI.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateCategory(Category category);

        Task<Category> FindCategory(Guid id);

        Task<Category?> UpdateCategoryById(Category category);

        Task<Category?> DeleteCategory(Guid id);
        Task<UserSummary> GetUserSummary(int userid);

        Task<PaginatedResult<Category>> GetFilteredAsync(int userId, int month, string type, string paymentMethod, string sortBy, string sortOrder,int pageNumber,int pageSize);
    }
}
