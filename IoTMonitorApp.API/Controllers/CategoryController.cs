using IoTMonitorApp.API.Dto;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                if (categories == null || !categories.Any())
                {
                    return NotFound("No categories found.");
                }
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving categories: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving category: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest("Category cannot be null");
            }
            try
            {
                await _categoryService.AddCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategoryById), category);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding category: " + ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest("Invalid category data.");
            }
            try
            {
                var result = await _categoryService.UpdateCategoryAsync(id, category);
                if (result == "Category not found")
                {
                    return NotFound($"Category {category.Name} not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating category: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting category: " + ex.Message);
            }
        }
    }
}