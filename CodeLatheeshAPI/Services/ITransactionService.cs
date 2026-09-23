using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace CodeLatheeshAPI.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransaction(Transaction transaction);

        Task<Transaction> FindTransaction(Guid id);

        Task<Transaction?> UpdateTransactionById(Transaction transaction);

        Task<Transaction?> DeleteTransaction(Guid id);
        Task<UserSummary> GetUserSummary(int userid,int selectedmonth);

        Task<PaginatedResult<Transaction>> GetFilteredAsync(int userId, int month, string type, string paymentMethod, string sortBy, string sortOrder,int pageNumber,int pageSize);
    }
}
