using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using CodeLatheeshAPI.Repositories.IRepository;
using Serilog;

namespace CodeLatheeshAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public Task<Category> CreateCategory(Category category)
        {
            try
            {
                var categorylist = _categoryRepo.CreateAsync(category);
                return categorylist;

            }
            catch (Exception ex)
            {
                // Log only the exception message
                Log.Error("Error occurred: {Message}", ex.Message);

                // Optionally, log full exception (stack trace etc.)
                Log.Error(ex, "Full exception logged.");
                return null;
            }
        }

        public Task<Category> FindCategory(Guid id)
        {
            return _categoryRepo.FindByIdAsync(id);
        }

        public Task<Category?> UpdateCategoryById(Category category)
        {
            return _categoryRepo.UpdateCategoryById(category);
        }

        public Task<Category?> DeleteCategory(Guid id)
        {
            return _categoryRepo.DeleteAsync(id);
        }

        public Task<UserSummary> GetUserSummary(int userid)
        {
            return _categoryRepo.GetUserSummaryAsync(userid);
        }

        public Task<PaginatedResult<Category>> GetFilteredAsync(
        int userId,
        int month,
        string type,
        string paymentMethod,
        string sortBy,
        string sortOrder,
        int pageNumber = 1,
        int pageSize = 10)
        {
            return _categoryRepo.GetFilteredAsync(userId,month,type,paymentMethod,sortBy,sortOrder,pageNumber,pageSize);
        }
    }
}
