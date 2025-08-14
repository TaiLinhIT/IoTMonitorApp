using IoTMonitorApp.API.Dto.Proudct;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products); // 200 + JSON
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound(); // 404
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var newProduct = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { newProduct });
            // 201 Created + link GET
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProductDto dto)
        {
            if (id != dto.Id) return BadRequest(); // 400

            var success = await _productService.UpdateAsync(dto);
            if (!success) return NotFound(); // 404

            return NoContent(); // 204
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _productService.DeleteAsync(id);
            if (!success) return NotFound(); // 404
            return NoContent(); // 204
        }
    }
}

