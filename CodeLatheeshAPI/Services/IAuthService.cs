using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
namespace CodeLatheeshAPI.Services
{
    public interface IAuthService
    {
        public string GenerateToken(string username);

        Task<UserDTO> RegisterUser(register user);

        Task<Users> GetUser(login loginuser);
    }
}
