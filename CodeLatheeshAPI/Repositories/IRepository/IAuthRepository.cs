using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;

namespace CodeLatheeshAPI.Repositories.IRepository
{
    public interface IAuthRepository
    {
       Task<Users> CreateAsync(Users users);

        Task<Users> GetUserAsync(login loginusers);
    }
}
