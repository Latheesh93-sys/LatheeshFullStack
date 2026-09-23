using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using CodeLatheeshAPI.Repositories.IRepository;
using Serilog;

namespace CodeLatheeshAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        public TransactionService(ITransactionRepository transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }
        public Task<Transaction> CreateTransaction(Transaction transaction)
        {
            try
            {
                var transactionlist = _transactionRepo.CreateAsync(transaction);
                return transactionlist;

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

        public Task<Transaction> FindTransaction(Guid id)
        {
            return _transactionRepo.FindByIdAsync(id);
        }

        public Task<Transaction?> UpdateTransactionById(Transaction transaction)
        {
            return _transactionRepo.UpdateById(transaction);
        }

        public Task<Transaction?> DeleteTransaction(Guid id)
        {
            return _transactionRepo.DeleteAsync(id);
        }

        public Task<UserSummary> GetUserSummary(int userid, int selectedmonth)
        {
            return _transactionRepo.GetUserSummaryAsync(userid,selectedmonth);
        }

        public Task<PaginatedResult<Transaction>> GetFilteredAsync(
        int userId,
        int month,
        string type,
        string paymentMethod,
        string sortBy,
        string sortOrder,
        int pageNumber = 1,
        int pageSize = 10)
        {
            return _transactionRepo.GetFilteredAsync(userId,month,type,paymentMethod,sortBy,sortOrder,pageNumber,pageSize);
        }
    }
}
