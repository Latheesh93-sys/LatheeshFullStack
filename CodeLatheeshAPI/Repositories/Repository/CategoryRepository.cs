using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Models.DomainModels;
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
         
        public async Task<IEnumerable<Category>> GetAllAsync(int userId)
        {
           return await dbContext.Categories.Where(c => c.UserId == userId).ToListAsync();
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
    }
}
