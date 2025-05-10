using CodeLatheeshAPI.Models.DomainModels;
using CodeLatheeshAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodeLatheeshAPI.Data;
using CodeLatheeshAPI.Repositories.IRepository;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CodeLatheeshAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            await categoryRepository.CreateAsync(category);
            var response = new CategoryDto
            {
                Id=category.Id,
                Name=category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id= category.Id,
                    Name=category.Name,
                    UrlHandle=category.UrlHandle
                });
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var selectedCategory = await categoryRepository.FindByIdAsync(id);
            if(selectedCategory is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = selectedCategory.Id,
                Name = selectedCategory.Name,
                UrlHandle = selectedCategory.UrlHandle
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
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };
            category = await categoryRepository.UpdateCategoryById(category);
            if (category is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };
            return Ok(response);
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
