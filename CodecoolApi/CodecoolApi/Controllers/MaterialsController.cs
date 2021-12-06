using AutoMapper;
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

        public MaterialsController(ILogger<MaterialsController> logger, IMapper mapper, IRepository<Material> materialRepository, IRepository<Review> reviewRepository)
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
            var result = await _materialRepository.GetAllAsync();

            if (result == null)
            {
                _logger.LogInformation($"Actors NotFound");
                return NotFound();
            }
            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<Material> materials = await result.ToListAsync();
            return Ok(materials);
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
                _logger.LogInformation($"Actor NotFound");
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
    }
}
