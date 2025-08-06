using Xunit;
using Moq;
using CodeLatheeshAPI.Repositories.IRepository;
using CodeLatheeshAPI.Services;
using CodeLatheeshAPI.Models.DomainModels;
using Microsoft.Extensions.Configuration;
namespace CodeLatheeshAPI.Tests;

public class AuthServiceTests
{
    private readonly Mock<IAuthRepository> _mockRepo;
    private readonly AuthService _authService;
    private readonly Mock<IConfiguration> _mockConfig;
    public AuthServiceTests()
    {
        _mockRepo = new Mock<IAuthRepository>();
        _mockConfig = new Mock<IConfiguration>(); 
        _authService = new AuthService(_mockConfig.Object, _mockRepo.Object);
    }

    [Fact]
    public async Task GetUser_ReturnsExpectedUser()
    {
        // Arrange
        var loginRequest = new login
        {
            Username = "testuser",
            Password = "password123"
        };

        var expectedUser = new Users
        {
            Username = "testuser",
            Password = "password123",
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            UserId = 18
        };

        _mockRepo.Setup(repo => repo.GetUserAsync(loginRequest))
                 .ReturnsAsync(expectedUser);

        // Act
        var result = await _authService.GetUser(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("Test", result.FirstName);
        Assert.Equal("User", result.LastName);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal(18, result.UserId);
    }
}
