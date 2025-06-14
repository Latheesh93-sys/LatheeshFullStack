using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Repositories.IRepository;
using CodeLatheeshAPI.Repositories.Repository;
using CodeLatheeshAPI.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Azure.Core;

namespace CodeLatheeshAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;
        public AuthService(IConfiguration config,IAuthRepository authRepository)
        {
            _config = config;
            _authRepository = authRepository;

        }
        public string GenerateToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin") // Example Role
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserDTO> RegisterUser(register users)
        {
            var passwordHasher = new PasswordHasher<register>();
            users.Password = passwordHasher.HashPassword(users, users.Password);
            var user = new Users
            {
                Username = users.Username,
                Password = users.Password,
                FirstName = users.FirstName,
                LastName = users.LastName,
                Email = users.Email,
                Categories = new List<Category>() // optional, can also be null
            };
            Users userDetails = await _authRepository.CreateAsync(user);
            var UserDetailsDto = new UserDTO
            {
                
                Username = user.Username,
                Token = GenerateToken(userDetails.Username),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserId = user.UserId

            };
            return UserDetailsDto;
        }

        public async Task<Users> GetUser(login user)
        {
            Users loginuser = await _authRepository.GetUserAsync(user);
            var loginUserDetails = new Users
            {
                Username = loginuser.Username,
                Password= loginuser.Password,
                FirstName= loginuser.FirstName,
                LastName= loginuser.LastName,
                Email= loginuser.Email,
                UserId=loginuser.UserId
            };
            return loginUserDetails;
        }
    }
}
