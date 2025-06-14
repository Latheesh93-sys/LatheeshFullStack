using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using CodeLatheeshAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeLatheeshAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] login loginUser)
        {
            var passwordHasher = new PasswordHasher<login>();
            var user = await _authService.GetUser(loginUser);
            var verify = passwordHasher.VerifyHashedPassword(loginUser, user.Password,loginUser.Password);
            if (verify==PasswordVerificationResult.Success) 
            {
                var token = _authService.GenerateToken(loginUser.Username);
                var UserDetailsDto = new UserDTO
                {
                    Username = user.Username,
                    Token = token,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email=user.Email,
                    UserId=user.UserId

                };
                return Ok(new { UserDetailsDto });
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] register register)
        {
            var userDetailsDto = await _authService.RegisterUser(register);

            return Ok(new { userDetailsDto });

            
        }

       
    }
}
