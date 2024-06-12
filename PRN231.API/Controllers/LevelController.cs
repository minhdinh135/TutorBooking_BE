using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelController : ControllerBase
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;
        private readonly ILogger<LevelController> _logger;
        public IConfiguration _configuration;

        public LevelController(IConfiguration config, ILogger<LevelController> logger,
                IGenericService<Level, LevelDTO> levelService)
        {
            _logger = logger;
            _configuration = config;
            _levelService = levelService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var levelList = await _levelService.GetAll();
            return Ok(levelList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var levelList = await _levelService.Get(id);
            return Ok(levelList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(LevelDTO dto)
        {
            var level = await _levelService.Add(dto);
            return Ok(level);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(LevelDTO dto)
        {
            var level = await _levelService.Update(dto);

            if (level == null)
            {
                return NotFound($"Level with ID {dto.Id} not found.");
            }

            level.LevelName = dto.LevelName;
            return Ok(level);
        }

    }
}
