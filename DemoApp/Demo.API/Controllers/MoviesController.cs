using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Demo.API.Contracts;
using Demo.API.Data;
using Demo.API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public MoviesController(IMovieRepository movieRepository, IMapper mapper, ILoggerService logger)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// List of Directors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovies()
        {
            try
            {
                _logger.LogInfo("Get all movies");
                var movies = await _movieRepository.FindAll();
                var response = _mapper.Map<IList<MovieDTO>>(movies);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }

        }
        /// <summary>
        /// Get Movie by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovies(int id)
        {
            try
            {
                _logger.LogInfo($"Get single movie with id: {id}");
                var movie = await _movieRepository.FindById(id);
                if (movie == null)
                {
                    _logger.LogWarn($"Movie with id {id} is not found");
                    return NotFound();
                }
                var response = _mapper.Map<MovieDTO>(movie);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }

        }

        /// <summary>
        /// Creates a movie   
        /// </summary>
        /// <param name="movieDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MovieCreateDTO movieDTO)
        {
            try
            {
                _logger.LogInfo("Create movie started");
                if (movieDTO == null)
                {
                    _logger.LogWarn("Empty request");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Data incomplete");
                    return BadRequest(ModelState);
                }
                var movie = _mapper.Map<Movie>(movieDTO);
                var isSuccess = await _movieRepository.Create(movie);
                if (!isSuccess)
                {
                    return InternalError("Movie creation failed}");
                }
                _logger.LogInfo("Movie created");
                return Created("Create", new { movie });
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a movie   
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] MovieUpdateDTO movieDTO)
        {
            try
            {
                _logger.LogInfo("Update started");
                if (id < 1 || movieDTO == null || id != movieDTO.Id)
                {
                    _logger.LogWarn("Empty request");
                    return BadRequest();
                }
                var isExists = await _movieRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn("Movie does not exists");
                    return BadRequest();
                }
                var movie = _mapper.Map<Movie>(movieDTO);
                var isSuccess = await _movieRepository.Update(movie);
                if (!isSuccess)
                {
                    return InternalError("Movie update failed");
                }
                _logger.LogInfo("Movie updated");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a movie   
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
                var isExists = await _movieRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn("Movie does not exists");
                    return BadRequest();
                }
                var movie = await _movieRepository.FindById(id);
                if (movie == null)
                {
                    _logger.LogWarn("Director not found");
                    return NotFound();
                }
                var isSuccess = await _movieRepository.Delete(movie);
                if (!isSuccess)
                {
                    return InternalError("Movie deletion failed");
                }
                _logger.LogInfo("Movie deleted");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError($"Something went wrong: {message}");
            return StatusCode(500, "Something went wrong");
        }
    }
}
