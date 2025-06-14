using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using CodeLatheeshAPI.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CodeLatheeshAPI.Repositories.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext dbContext;
        public AuthRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Users> CreateAsync(Users users)
        {
            await dbContext.Users.AddAsync(users);
            await dbContext.SaveChangesAsync();
            return users;
        }
        public async Task<Users> GetUserAsync(login loginusers)
        {
            return await dbContext.Users.FirstAsync(c => c.Username == loginusers.Username);
        }
    }
}
