using AutoMapper;
using CodecoolApi.DAL.DTO.MaterialType;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodecoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsTypesController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<MaterialType> _materialTypeRepository;

        public MaterialsTypesController(ILogger<MaterialsController> logger, IRepository<MaterialType> materialTypeRepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _materialTypeRepository = materialTypeRepository;
        }

        /// <summary>
        /// Get List of material types
        /// </summary>
        [SwaggerOperation(Summary = "Get all material types")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMaterialsTypes()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialTypeRepository.GetAllAsync();

            if (result == null)
            {
                _logger.LogInformation($"Authors NotFound");
                return NotFound();
            }
            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<MaterialType> materialTypes = await result.ToListAsync();
            return Ok(materialTypes);
        }

        /// <summary>
        /// Get material type with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get material type with specific id")]
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MaterialType>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMaterialTypeById(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialTypeRepository.GetAsync(id);

            if (result == null)
            {
                _logger.LogInformation($"Author NotFound");
                return NotFound();
            }

            _logger.LogInformation($"Return {result} of {result.GetType()}");
            return Ok(result);
        }

        /// <summary>
        /// Post new material type
        /// </summary>
        [SwaggerOperation(Summary = "Post a new material type")]
        [HttpPost]
        public async Task<IActionResult> PostMaterialType(PostMaterialTypeDto materialType)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            await _materialTypeRepository.CreateAsync(_mapper.Map<MaterialType>(materialType));
            return Ok();
        }

        /// <summary>
        /// Delete existing material type
        /// </summary>
        [SwaggerOperation(Summary = "Delete existing material type")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterialType(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialTypeRepository.GetEntityByQuery(x => x.Id == id);

            if (result == null)
            {
                _logger.LogInformation($"Entity not found");
                return NotFound();
            }

            await _materialTypeRepository.DeleteAsync(result);
            _logger.LogInformation($"Deleted {result.GetType()}");
            return Ok();
        }

        /// <summary>
        /// Change existing material type
        /// </summary>
        [SwaggerOperation(Summary = "Change existing material type")]
        [HttpPut]
        public async Task<IActionResult> PutMaterialType(MaterialType materialType)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            if (materialType == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }

            await _materialTypeRepository.UpdateAsync(materialType);
            _logger.LogInformation($"Material type changed");
            return Ok();
        }
    }
}
