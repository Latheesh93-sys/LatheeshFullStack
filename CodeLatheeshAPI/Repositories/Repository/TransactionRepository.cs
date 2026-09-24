using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using CodeLatheeshAPI.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeLatheeshAPI.Repositories.Repository
{
    public class TransactionRepository:ITransactionRepository
    {
        private readonly ApplicationDbContext dbContext;
        public TransactionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            await dbContext.Transactions.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> DeleteAsync(Guid id)
        {
           var existingTransaction = await dbContext.Transactions.FirstOrDefaultAsync(c => c.Id == id);
            if (existingTransaction is null)
            {
                return null;
            }
            dbContext.Transactions.Remove(existingTransaction);
            await dbContext.SaveChangesAsync();
            return existingTransaction;
        }

        public async Task<Transaction> FindByIdAsync(Guid id)
        {
            return await dbContext.Transactions.FirstAsync(c => c.Id == id);
        }

        public async Task<PaginatedResult<Transaction>> GetFilteredAsync(
        int userId,
        int month,
        string type,
        string paymentMethod,
        string sortBy,
        string sortOrder,
        int pageNumber = 1,
        int pageSize = 10)
        {
            var query = dbContext.Transactions.AsQueryable();

            // Filter by user
            query = query.Where(c => c.UserId == userId);

            // Filter by month (if provided)
            if (month == 0)
            {
                month = DateTime.Now.Month;
            }
            query = query.Where(c => c.Date.Month == month);
            

            // Filter by type (if provided)
            if (!string.IsNullOrEmpty(type) && type !="All")
            {
                query = query.Where(c => c.Type == type);
            }

            // Filter by payment method (if provided)
            if (!string.IsNullOrEmpty(paymentMethod) && paymentMethod!="All")
            {
                query = query.Where(c => c.PaymentMethod == paymentMethod);
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "date":
                        query = sortOrder == "desc"
                            ? query.OrderByDescending(c => c.Date)
                            : query.OrderBy(c => c.Date);
                        break;

                    case "amount":
                        query = sortOrder == "desc"
                            ? query.OrderByDescending(c => c.Amount)
                            : query.OrderBy(c => c.Amount);
                        break;
                }
            }

            // Total count before pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Transaction>
            {
                Items = items,
                TotalCount = totalCount
            };
        }


        public async Task<Transaction?> UpdateById(Transaction transaction)
        {
            var existingTransaction= await dbContext.Transactions.FirstOrDefaultAsync(c => c.Id == transaction.Id);
            if (existingTransaction != null)
            {
                dbContext.Entry(existingTransaction).CurrentValues.SetValues(transaction);
                await dbContext.SaveChangesAsync();
                return transaction;
            }
            return null;
        }

        public async Task<UserSummary> GetUserSummaryAsync(int userId, int selectedmonth)
        {
            var now = DateTime.Now;

            // Query income this month, expense,investment and current balance for the current user (and optionally current month)
            var totalIncome = await dbContext.Transactions
                .Where(c => c.UserId == userId && c.Type == "Income" && c.Date.Year == now.Year
                && c.Date.Month == selectedmonth)
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var totalExpense = await dbContext.Transactions
                .Where(c => c.UserId == userId && c.Type == "Expense" && c.Date.Year == now.Year
                && c.Date.Month == selectedmonth)
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var totalInvestment = await dbContext.Transactions
                .Where(c => c.UserId == userId && c.Type == "Investment" && c.Date.Year == now.Year 
                && c.Date.Month == selectedmonth)
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var currentAccountBalance = await dbContext.Transactions.Where(c => c.UserId == userId && c.Type == "Income")
                                                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            // Get top expenses ordered by amount descending, take top 5 (or any count you want)
            var topExpenses = await dbContext.Transactions
                .Where(c => c.UserId == userId && c.Type == "Expense" && c.Date.Year == now.Year && c.Date.Month == selectedmonth)
                .OrderByDescending(c => c.Amount)
                .Take(5)
                .Select(c => new TransactionDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Amount = c.Amount,
                    Date = c.Date.ToString("dd-MM-yyyy")
                    // Map other needed properties here
                })
                .ToListAsync();

            return new UserSummary
            {
                TotalIncome = totalIncome,
                CurrentBalance = currentAccountBalance,
                TotalExpense = totalExpense,
                TotalInvestment = totalInvestment,
                TopExpenses = topExpenses
            };
        }

    }
}
