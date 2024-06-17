using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Repository.Interfaces;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelController : ControllerBase
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;
        private readonly IGenericRepository<Level> _levelRepo;
        private readonly ILogger<LevelController> _logger;
        public IConfiguration _configuration;

        public LevelController(IConfiguration config, ILogger<LevelController> logger,
                IGenericService<Level, LevelDTO> levelService,
                IGenericRepository<Level> levelRepo)
        {
            _logger = logger;
            _configuration = config;
            _levelService = levelService;
            _levelRepo = levelRepo;

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
            if (ModelState.IsValid == false)
            {
                return BadRequest(ErrorHandler.GetErrorMessage(ModelState));
            }

            try
            {
                var level = await _levelService.Add(dto);
         
                return Ok(level);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(LevelDTO dto)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ErrorHandler.GetErrorMessage(ModelState));
            }

            try
            {
                var existingLevels = await _levelService.GetAll();
                existingLevels= existingLevels.Where(e => e.LevelName == dto.LevelName && e.Id != dto.Id);
                if (existingLevels.Any())
                {
                    return BadRequest("This name is existed");
                }
                var levels = await _levelService.Get(dto.Id);
                levels.LevelName = dto.LevelName; 
                levels.Status = dto.Status;
                var level = await _levelRepo.Update(levels);
                return Ok(level);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
