using Education.DTO;
using Education.Models;
using Education.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(dto);
                return Ok(new { status = "Success", message = "Category created.", data = category });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
    }
}
