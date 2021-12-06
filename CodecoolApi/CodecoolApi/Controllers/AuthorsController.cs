﻿using AutoMapper;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodecoolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILogger<MaterialsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<Author> _authorRepository;

        public AuthorsController(ILogger<MaterialsController> logger, IRepository<Author> authorRepository, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        /// <summary>
        /// Get List of authors
        /// </summary>
        [SwaggerOperation(Summary = "Get all authors")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllMaterials()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var result = await _authorRepository.GetAllAsync();

            if (result == null)
            {
                _logger.LogInformation($"Authors NotFound");
                return NotFound();
            }
            _logger.LogInformation($"Return {await result.CountAsync()} of {result.GetType()}");
            List<Author> authors = await result.ToListAsync();
            return Ok(authors);
        }

        /// <summary>
        /// Get author with specific id
        /// </summary>
        [SwaggerOperation(Summary = "Get author with specific id")]
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Author>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        public async Task<IActionResult> PostAuthor()
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            await _authorRepository.CreateAsync(new Author());
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
        public async Task<IActionResult> PutAuthor(Author author)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            if (author == null)
            {
                _logger.LogInformation($"BadRequest");
                return BadRequest();
            }

            await _authorRepository.UpdateAsync(author);
            _logger.LogInformation($"Author changed");
            return Ok();
        }
    }
}
