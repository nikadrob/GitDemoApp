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
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public DirectorsController(IDirectorRepository directorRepository, IMapper mapper, ILoggerService logger)
        {
            _directorRepository = directorRepository;
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
        public async Task<IActionResult> GetDirectors()
        {
            try
            {
                _logger.LogInfo("Get all directors");
                var directors = await _directorRepository.FindAll();
                var response = _mapper.Map<IList<DirectorDTO>>(directors);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }

        }
        /// <summary>
        /// Get Director by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDirectors(int id)
        {
            try
            {
                _logger.LogInfo($"Get single director with id: {id}");
                var director = await _directorRepository.FindById(id);
                if (director == null)
                {
                    _logger.LogWarn($"Director with id {id} is not found");
                    return NotFound();
                }
                var response = _mapper.Map<DirectorDTO>(director);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }

        }

        /// <summary>
        /// Creates a director   
        /// </summary>
        /// <param name="directorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DirectorCreateDTO directorDTO)
        {
            try
            {
                _logger.LogInfo("Create started");
                if (directorDTO == null)
                {
                    _logger.LogWarn("Empty request");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Data incomplete");
                    return BadRequest(ModelState);
                }
                var director = _mapper.Map<Director>(directorDTO);
                var isSuccess = await _directorRepository.Create(director);
                if (!isSuccess)
                {
                    return InternalError("Director creation failed}");
                }
                _logger.LogInfo("Director created");
                return Created("Create", new { director });
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a director   
        /// </summary>
        /// <param name="id"></param>
        /// <param name="directorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] DirectorUpdateDTO directorDTO)
        {
            try
            {
                _logger.LogInfo("Update started");
                if (id < 1 || directorDTO == null || id != directorDTO.Id)
                {
                    _logger.LogWarn("Empty request");
                    return BadRequest();
                }
                var isExists = await _directorRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn("Director does not exists");
                    return BadRequest();
                }
                var director = _mapper.Map<Director>(directorDTO);
                var isSuccess = await _directorRepository.Update(director);
                if (!isSuccess)
                {
                    return InternalError("Director update failed");
                }
                _logger.LogInfo("Director updated");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"Something went wrong: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a director   
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
                var isExists = await _directorRepository.IsExists(id);
                if (!isExists)
                {
                    _logger.LogWarn("Director does not exists");
                    return BadRequest();
                }
                var director = await _directorRepository.FindById(id);
                if (director == null)
                {
                    _logger.LogWarn("Director not found");
                    return NotFound();
                }
                var isSuccess = await _directorRepository.Delete(director);
                if (!isSuccess)
                {
                    return InternalError("Director deletion failed");
                }
                _logger.LogInfo("Director deleted");
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
