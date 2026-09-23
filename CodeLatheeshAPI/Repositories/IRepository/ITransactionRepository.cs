using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;

namespace CodeLatheeshAPI.Repositories.IRepository
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);

        Task<Transaction> FindByIdAsync(Guid id);  

        Task<Transaction?> UpdateById(Transaction transaction);

        Task<Transaction?> DeleteAsync(Guid id);

        Task<UserSummary> GetUserSummaryAsync(int userId,int selectedmonth);

        Task<PaginatedResult<Transaction>> GetFilteredAsync(
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
