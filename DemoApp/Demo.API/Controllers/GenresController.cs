using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.API.Contracts;
using Demo.API.Data;
using Demo.API.DTO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GenresController(IGenreRepository genreRepository, IMapper mapper, ILoggerService logger)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGenres()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Started Get All Genres");
                var genres = await _genreRepository.FindAll();
                var response = _mapper.Map<IList<GenreDTO>>(genres);
                _logger.LogInfo($"{location}: Successful");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location} : Something wrong {ex.Message}");
            }
        }

        /// <summary>
        /// Get single genre by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGenre(int Id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Started Get Single Genre");
                var genre = await _genreRepository.FindById(Id);
                if (genre == null)
                {
                    _logger.LogWarn($"{location}: No exists id {Id}");
                    return NotFound();
                }
                var response = _mapper.Map<GenreDTO>(genre);
                _logger.LogInfo($"{location}: Successful");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{location} : Something wrong {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a genre
        /// </summary>
        /// <param name="genreDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] GenreCreateDTO genreDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Started Create Genre");
                if (genreDTO == null)
                {
                    _logger.LogError($"{location}: Empty request");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"{location}: Wrong model state");
                    return BadRequest(ModelState);
                }
                var genre = _mapper.Map<Genre>(genreDTO);
                var IsSuccess = await _genreRepository.Create(genre);
                if (!IsSuccess)
                {
                    return InternalError($"{location} : Creation failed");
                }
                return Created("Create", new { genre });
            }
            catch (Exception ex)
            {
                return InternalError($"{location} : Something wrong {ex.Message}");
            }
        }

        /// <summary>
        /// Genre updating
        /// </summary>
        /// <param name="id"></param>
        /// <param name="genreDTO"></param>
        /// <returns>No content</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] GenreUpdateDTO genreDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Started update genre");
                if (id < 1 || genreDTO == null || id != genreDTO.Id)
                {
                    _logger.LogError($"{location}: Empty request");
                    return BadRequest();
                }
                var isExists = await _genreRepository.IsExists(id);
                if(!isExists)
                {
                    _logger.LogError($"{location}: Wrong Id");
                    return BadRequest();
                }
                var author = _mapper.Map<Genre>(genreDTO);
                var isSuccess = await _genreRepository.Update(author);
                if(!isSuccess)
                {
                    return InternalError("Genre update failed");
                }
                _logger.LogInfo("Genre updated");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{location} : Something wrong {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a genre   
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo("Delete started");
                if (id < 1)
                {
                    _logger.LogWarn("Empty request");
                    return BadRequest();
                }
                var isExists = await _genreRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn("Genre does not exists");
                    return BadRequest();
                }
                var genre = await _genreRepository.FindById(id);
                if (genre == null)
                {
                    _logger.LogWarn("Genre not found");
                    return NotFound();
                }
                var isSuccess = await _genreRepository.Delete(genre);
                if (!isSuccess)
                {
                    return InternalError("Genre deletion failed");
                }
                _logger.LogInfo("Genre deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError($"Something went wrong: {message}");
            return StatusCode(500, "Something went wrong");
        }
    }
}
