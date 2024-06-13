using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;
        private readonly IGenericService<Subject, SubjectDTO> _subjectService;
        private readonly ILogger<SubjectController> _logger;
        public IConfiguration _configuration;

        public SubjectController(IConfiguration config, ILogger<SubjectController> logger,
                IGenericService<Subject, SubjectDTO> subjectService,
                IGenericService<Level, LevelDTO> levelService)
        {
            _logger = logger;
            _configuration = config;
            _subjectService = subjectService;
            _levelService = levelService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var subjectList = await _subjectService.GetAll();
            return Ok(subjectList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var subjectList = await _subjectService.Get(id);
            return Ok(subjectList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(SubjectDTO dto)
        {
            var subject = await _subjectService.Add(dto);
      
            return Ok(subject);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(SubjectDTO dto)
        {
            var subject = await _subjectService.Update(dto);
            return Ok(subject);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var subject = await _subjectService.Delete(id);
            return Ok(subject);
        }
    }
}

