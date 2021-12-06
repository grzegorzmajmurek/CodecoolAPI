using AutoMapper;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodecoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Review> _reviewRepository;

        public ReviewsController(ILogger<MaterialsController> logger, IRepository<Review> reviewRepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Get List of reviews
        /// </summary>
        [SwaggerOperation(Summary = "Get all reviews")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllReviews()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _reviewRepository.GetAllAsync();

            if (result == null)
            {
                _logger.LogInformation($"Reviews NotFound");
                return NotFound();
            }
            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<Review> reviews = await result.ToListAsync();
            return Ok(reviews);
        }

        /// <summary>
        /// Get review with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get review with specific id")]
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Review>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviewById(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _reviewRepository.GetAsync(id);

            if (result == null)
            {
                _logger.LogInformation($"Review NotFound");
                return NotFound();
            }

            _logger.LogInformation($"Return {result} of {result.GetType()}");
            return Ok(result);
        }

        /// <summary>
        /// Post new review
        /// </summary>
        [SwaggerOperation(Summary = "Post a new review")]
        [HttpPost]
        public async Task<IActionResult> PostReview()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            await _reviewRepository.CreateAsync(new Review());
            return Ok();
        }

        /// <summary>
        /// Delete existing review
        /// </summary>
        [SwaggerOperation(Summary = "Delete existing review")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _reviewRepository.GetEntityByQuery(x => x.Id == id);

            if (result == null)
            {
                _logger.LogInformation($"Entity not found");
                return NotFound();
            }

            await _reviewRepository.DeleteAsync(result);
            _logger.LogInformation($"Deleted {result.GetType()}");
            return Ok();
        }

        /// <summary>
        /// Change existing review
        /// </summary>
        [SwaggerOperation(Summary = "Change existing review")]
        [HttpPut]
        public async Task<IActionResult> PutReview(Review review)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            if (review == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }

            await _reviewRepository.UpdateAsync(review);
            _logger.LogInformation($"Review type changed");
            return Ok();
        }
    }
}
