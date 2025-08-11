using IoTMonitorApp.API.Dto.Specification;
using IoTMonitorApp.API.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitorApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecificationService _specificationService;
        public SpecificationController(ISpecificationService specificationService)
        {
            _specificationService = specificationService;
        }
        // GET: api/Specification
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var specifications = await _specificationService.GetAllAsync();
                if (specifications == null || !specifications.Any())
                {
                    return NotFound("No specifications found.");
                }
                return Ok(specifications);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Specification/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var specification = await _specificationService.GetSpecificationByIdAsync(id);
                if (specification == null)
                {
                    return NotFound($"Specification with id {id} not found.");
                }
                return Ok(specification);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Specification
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SpecificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _specificationService.AddSpecificationAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Specification
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SpecificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _specificationService.UpdateSpecificationAsync(dto);
                if (result == "Specification not found.")
                {
                    return NotFound(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Specification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _specificationService.DeleteSpecificationAsync(id);
                if (!deleted)
                {
                    return NotFound($"Specification with id {id} not found.");
                }
                return NoContent(); // 204
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
