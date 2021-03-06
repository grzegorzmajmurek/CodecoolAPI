using AutoMapper;
using CodecoolApi.DAL.DTO.Material;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CodecoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Material> _materialRepository;
        private readonly IRepository<Review> _reviewRepository;

        public MaterialsController(ILogger<MaterialsController> logger, IMapper mapper, IRepository<Material> materialRepository, IRepository<Review> reviewRepository, IRepository<Author> authorRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _materialRepository = materialRepository;
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Get List of materials
        /// </summary>
        [SwaggerOperation(Summary = "Get all materials")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMaterials()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.EnlistAllEager(
                materials => materials
                .Include(x => x.Reviews)
                .Include(x => x.Type)
                .Include(x => x.Author));

            if (result == null)
            {
                _logger.LogInformation($"Materials NotFound");
                return NotFound();
            }
            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<Material> materials = await result.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<GetMaterialDto>>(materials));
        }

        /// <summary>
        /// Get material with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get material with specific id")]
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Material>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.GetAsync(id);

            if (result == null)
            {
                _logger.LogInformation($"Material NotFound");
                return NotFound();
            }

            _logger.LogInformation($"Return {result} of {result.GetType()}");
            return Ok(result);
        }

        /// <summary>
        /// Get all review from material with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get all review from material")]
        [HttpGet("{id}/Reviews/")]
        public async Task<IActionResult> GetReviewsMaterial(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.GetEntityByQueryEager(material => material.Include(material => material.Reviews), material => material.SingleOrDefault(material => material.Id == id));
            if (result == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Return {result} of {result.GetType()}");
            return Ok(result);
        }

        /// <summary>
        /// Get all authors from material with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get all authors from material")]
        [HttpGet("{id}/Authors/")]
        public async Task<IActionResult> GetAuthorsMaterial(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.GetEntityByQueryEager(material => material.Include(material => material.Reviews), material => material.SingleOrDefault(material => material.Id == id));
            if (result == null)
            {
                return NotFound();
            }
            _logger.LogInformation($"Return {result} of {result.GetType()}");
            return Ok(result);
        }

        /// <summary>
        /// Get review with specific id per material with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get all review per material")]
        [HttpGet("{id}/Reviews/{reviewId}")]
        public async Task<IActionResult> GetMaterialReviewById(int id, int reviewId)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            Material material = await _materialRepository.GetEntityByQueryEager(material => material.Include(material => material.Reviews), material => material.FirstOrDefault(material => material.Id == id));
            Review? review = GetReviewIfExist(reviewId, material);

            if (review == null)
            {
                _logger.LogInformation($"Review for given review id {reviewId} is not found");
                return NotFound();
            }
            _logger.LogInformation($"Return {review} of {review.GetType()}");
            return Ok(review);
        }

        private static Review? GetReviewIfExist(int reviewId, Material material)
        {
            Review? review = null;
            if (material.Reviews != null)
            {
                var result = material.Reviews.Where(x => x.Id == reviewId).FirstOrDefault();
                if (result != null)
                {
                    review = result;
                }
            }
            return review;
        }

        /// <summary>
        /// Post new material
        /// </summary>
        [SwaggerOperation(Summary = "Post a new material")]
        [HttpPost]
        public async Task<IActionResult> PostMaterial(PostMaterialDto material)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            await _materialRepository.CreateAsync(_mapper.Map<Material>(material));
            return Ok();
        }

        /// <summary>
        /// Delete existing materials
        /// </summary>
        [SwaggerOperation(Summary = "Delete existing materials")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.GetEntityByQuery(x => x.Id == id);

            if (result == null)
            {
                _logger.LogInformation($"Entity not found");
                return NotFound();
            }

            await _materialRepository.DeleteAsync(result);
            _logger.LogInformation($"Deleted {result.GetType()}");
            return Ok();
        }

        /// <summary>
        /// Delete review from materials
        /// </summary>
        [SwaggerOperation(Summary = "Delete review from materials")]
        [HttpDelete("{id}/Reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReviewFromMaterial(int id, int reviewId)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.GetEntityByQueryEager(material => material.Include(material => material.Reviews), material => material.SingleOrDefault(material => material.Id == id));

            Review? review = null;

            if (result.Reviews != null)
            {
                var material = result.Reviews.Where(x => x.Id == reviewId).FirstOrDefault();
                if (material != null)
                {
                    review = material;
                }
            }

            if (review == null)
            {
                _logger.LogInformation($"Entity not found");
                return NotFound();
            }

            if (result.Reviews != null && result.Reviews.Contains(review))
            {
                result.Reviews.Remove(review);
                await _materialRepository.UpdateAsync(result);
                _logger.LogInformation($"Deleted {result.GetType()}");
                return Ok();
            }

            return NotFound();
        }

        /// <summary>
        /// Change existing material
        /// </summary>
        [SwaggerOperation(Summary = "Change existing material")]
        [HttpPut]
        public async Task<IActionResult> PutMaterial(Material material)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            if (material == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }
            try
            {
                await _materialRepository.UpdateAsync(material);
                _logger.LogInformation($"Material changed");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error while updating materials");
                return StatusCode(409, "Error while updating materials.\r\n" + ex.Message.Split('.')[0]);
            }

            _logger.LogInformation($"Material modified");
            return Ok();
        }

        [SwaggerOperation(Summary = "Assign Review to Material")]
        [HttpPut("{id}/Reviews/{reviewId}")]
        public async Task<IActionResult> PutMaterialToAuthor(int id, int reviewId)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _materialRepository.GetEntityByQueryEager(x => x.Include(author => author.Reviews), x => x.SingleOrDefault(material => material.Id == id));
            var reviews = await _reviewRepository.GetEntityByQuery(x => x.Id == reviewId);

            if (result == null || reviews == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }

            if (result.Reviews == null)
                result.Reviews = new List<Review>();

            if (!result.Reviews.Contains(reviews))
            {
                result.Reviews.Add(reviews);
                await _materialRepository.UpdateAsync(result);
                _logger.LogInformation($"Review assigned to Material");
                return Ok();
            }

            return NotFound();
        }
    }
}
