using AutoMapper;
using CodecoolApi.DAL.DTO.Author;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace CodecoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Material> _materialRepository;

        public AuthorsController(ILogger<MaterialsController> logger, IRepository<Author> authorRepository, IRepository<Material> materialRepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _materialRepository = materialRepository;
        }

        /// <summary>
        /// Get List of authors
        /// </summary>
        [SwaggerOperation(Summary = "Get all authors")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAuthors()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _authorRepository.EnlistAllEager(authors =>
                  authors
                  .Include(author => author.Materials)
                );

            if (result == null)
            {
                _logger.LogInformation($"Authors NotFound");
                return NotFound();
            }

            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<Author> authors = await result.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<GetAuthorDto>>(authors));
        }

        /// <summary>
        /// Get author with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get author with specific id")]
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Author>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _authorRepository.GetAsync(id);

            if (result == null)
            {
                _logger.LogInformation($"Author NotFound");
                return NotFound();
            }

            _logger.LogInformation($"Return {result} of {result.GetType()}");
            return Ok(result);
        }

        /// <summary>
        /// Post new author
        /// </summary>
        [SwaggerOperation(Summary = "Post a new author")]
        [HttpPost]
        public async Task<IActionResult> PostAuthor(PostAuthorDto author)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            await _authorRepository.CreateAsync(_mapper.Map<Author>(author));
            return Ok();
        }

        /// <summary>
        /// Delete existing author
        /// </summary>
        [SwaggerOperation(Summary = "Delete existing author")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _authorRepository.GetEntityByQuery(x => x.Id == id);

            if (result == null)
            {
                _logger.LogInformation($"Entity not found");
                return NotFound();
            }

            await _authorRepository.DeleteAsync(result);
            _logger.LogInformation($"Deleted {result.GetType()}");
            return Ok();
        }

        /// <summary>
        /// Change existing author
        /// </summary>
        [SwaggerOperation(Summary = "Change existing author")]
        [HttpPut]
        public async Task<IActionResult> PutAuthor(PutAuthorDto author)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            if (author == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }

            await _authorRepository.UpdateAsync(_mapper.Map<Author>(author));
            _logger.LogInformation($"Author changed");
            return Ok();
        }

        [SwaggerOperation(Summary = "Assign Material to author")]
        [HttpPut("{id}/Materials/{materialId}")]
        public async Task<IActionResult> PutMaterialToAuthor(int id, int materialId)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _authorRepository.GetEntityByQueryEager(authors => authors.Include(author => author.Materials), authors => authors.SingleOrDefault(author => author.Id == id));
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
                await _authorRepository.UpdateAsync(result);
                _logger.LogInformation($"Material assigned to Author");
                return Ok();
            }

            return NotFound();
        }
    }
}
