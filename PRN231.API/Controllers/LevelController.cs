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
        private readonly IGenericService<Subject, SubjectDTO> _subjectService;
        private readonly IGenericRepository<SubjectLevel> _subjectLevelService;
        private readonly ILogger<LevelController> _logger;
        public IConfiguration _configuration;

        public LevelController(IConfiguration config, ILogger<LevelController> logger,
                IGenericService<Level, LevelDTO> levelService,
                IGenericService<Subject, SubjectDTO> subjectService,
                IGenericRepository<SubjectLevel> subjectLevelService)
        {
            _logger = logger;
            _configuration = config;
            _levelService = levelService;
            _subjectLevelService = subjectLevelService;
            _subjectService = subjectService;
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
            var subjects = await _subjectService.GetAll();
            foreach (var subject in subjects){
                var subjectLevel = new SubjectLevel
                {
                    LevelId = level.Id,
                    SubjectId = subject.Id,
                    Description = subject.Name + " " + level.LevelName,
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now

                };
                await _subjectLevelService.Add(subjectLevel);
            }
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
