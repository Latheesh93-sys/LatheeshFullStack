using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Repositories.IRepository;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using CodeLatheeshAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodeLatheeshAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                Type = request.Type,
                UserId=request.UserId,
                Amount=request.Amount,
                Date=request.Date,
                PaymentMethod=request.PaymentMethod
                
            };
            await _categoryService.CreateCategory(category);
            var response = new CategoryDto
            {
                Id=category.Id,
                Name=category.Name,
                UserId =category.UserId,
                Amount=category.Amount,
                Date=category.Date.ToString("dd-MM-yyyy"),
                PaymentMethod=category.PaymentMethod,
                Type=category.Type
            };
            return Ok(response);
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var selectedCategory = await _categoryService.FindCategory(id);
            if(selectedCategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = selectedCategory.Id,
                Name = selectedCategory.Name,
                UserId = selectedCategory.UserId,
                Amount = selectedCategory.Amount,
                Date = selectedCategory.Date.ToString("dd-MM-yyyy"),
                PaymentMethod = selectedCategory.PaymentMethod,
                Type = selectedCategory.Type
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            var category = new Category
            {
                Id = id,
                Type = request.Type,
                UserId = request.UserId,
                Amount = request.Amount,
                Date = request.Date,
                PaymentMethod = request.PaymentMethod,
                Name=request.Name

            };
            category = await _categoryService.UpdateCategoryById(category);
            if (category is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UserId = category.UserId,
                Amount = category.Amount,
                Date = category.Date.ToString("dd-MM-yyyy"),
                PaymentMethod = category.PaymentMethod,
                Type = category.Type
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _categoryService.DeleteCategory(id);
            if (category is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UserId = category.UserId,
                Amount = category.Amount,
                Date = category.Date.ToString("dd-MM-yyyy"),
                PaymentMethod = category.PaymentMethod,
                Type = category.Type
            };
            return Ok(response);
        }

        [HttpGet("usersummary/{userId}/{month}")]
        public async Task<IActionResult> GetUserSummary([FromRoute] int userId, [FromRoute] int month)
        {
            // Your logic here, for example:
            var summary = await _categoryService.GetUserSummary(userId, month);
            return Ok(summary);
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFiltered(
        [FromQuery] int userId,
        [FromQuery] int month,
        [FromQuery] string? type,
        [FromQuery] string? paymentMethod,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortOrder,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var result = await _categoryService.GetFilteredAsync(userId, month, type, paymentMethod, sortBy, sortOrder, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            try
            {
                // Simulate an error
                throw new Exception("This is a simulated exception for logging test.");
            }
            catch (Exception ex)
            {
                // Log only the exception message
                Log.Error("Error occurred: {Message}", ex.Message);

                // Optionally, log full exception (stack trace etc.)
                Log.Error(ex, "Full exception logged.");

                return StatusCode(500, "Error has been logged.");
            }
        }



    }
}
