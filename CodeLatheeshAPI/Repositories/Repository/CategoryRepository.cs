using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using CodeLatheeshAPI.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeLatheeshAPI.Repositories.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
           var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCategory is null)
            {
                return null;
            }
            dbContext.Categories.Remove(existingCategory);
            await dbContext.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<Category> FindByIdAsync(Guid id)
        {
            return await dbContext.Categories.FirstAsync(c => c.Id == id);
        }

        public async Task<PaginatedResult<Category>> GetFilteredAsync(
        int userId,
        int month,
        string type,
        string paymentMethod,
        string sortBy,
        string sortOrder,
        int pageNumber = 1,
        int pageSize = 10)
        {
            var query = dbContext.Categories.AsQueryable();

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

            return new PaginatedResult<Category>
            {
                Items = items,
                TotalCount = totalCount
            };
        }


        public async Task<Category?> UpdateCategoryById(Category category)
        {
            var existingCategory= await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<UserSummary> GetUserSummaryAsync(int userId, int selectedmonth)
        {
            var now = DateTime.Now;

            // Query total income, expense, and investment for the current user (and optionally current month)
            var totalIncome = await dbContext.Categories
                .Where(c => c.UserId == userId && c.Type == "Income" && c.Date.Year == now.Year
                && c.Date.Month == selectedmonth)
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var totalExpense = await dbContext.Categories
                .Where(c => c.UserId == userId && c.Type == "Expense" && c.Date.Year == now.Year
                && c.Date.Month == selectedmonth)
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            var totalInvestment = await dbContext.Categories
                .Where(c => c.UserId == userId && c.Type == "Investment" && c.Date.Year == now.Year 
                && c.Date.Month == selectedmonth)
                .SumAsync(c => (decimal?)c.Amount) ?? 0;

            // Get top expenses ordered by amount descending, take top 5 (or any count you want)
            var topExpenses = await dbContext.Categories
                .Where(c => c.UserId == userId && c.Type == "Expense" && c.Date.Year == now.Year && c.Date.Month == selectedmonth)
                .OrderByDescending(c => c.Amount)
                .Take(5)
                .Select(c => new CategoryDto
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
                TotalExpense = totalExpense,
                TotalInvestment = totalInvestment,
                TopExpenses = topExpenses
            };
        }

    }
}
