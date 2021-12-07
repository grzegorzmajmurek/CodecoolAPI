using AutoMapper;
using CodecoolApi.DAL.DTO.MaterialType;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Material> _materialRepository;

        public MaterialsTypesController(ILogger<MaterialsController> logger, IRepository<MaterialType> materialTypeRepository, IRepository<Material> materialRepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _materialTypeRepository = materialTypeRepository;
            _materialRepository = materialRepository;
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
            var result = await _materialTypeRepository.EnlistAllEager(x =>
                  x.Include(type => type.Materials)
                );


            if (result == null)
            {
                _logger.LogInformation($"Authors NotFound");
                return NotFound();
            }
            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<MaterialType> materialTypes = await result.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<GetMaterialTypeDto>>(materialTypes));
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

            try
            {
                await _materialTypeRepository.UpdateAsync(materialType);
                _logger.LogInformation($"Material type changed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating material types");
                return StatusCode(409, "Error while updating material types.\r\n" + ex.Message.Split('.')[0]);
            }
            _logger.LogInformation($"Material type changed");
            return Ok();
        }

        [SwaggerOperation(Summary = "Assign material to material type")]
        [HttpPut("{id}/Materials/{materialId}")]
        public async Task<IActionResult> PutMaterialToMaterialType(int id, int materialId)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialTypeRepository.GetEntityByQueryEager(x => x.Include(material => material.Materials), x => x.SingleOrDefault(material => material.Id == id));
            var materials = await _materialRepository.GetEntityByQuery(x => x.Id == materialId);

            if (result == null || materials == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }

            if (result.Materials == null)
                result.Materials = new List<Material>();

            if (!result.Materials.Contains(materials))
            {
                result.Materials.Add(materials);
                await _materialTypeRepository.UpdateAsync(result);
                _logger.LogInformation($"Material assigned to Material Type");
                return Ok();
            }

            return NotFound();
        }
    }
}
