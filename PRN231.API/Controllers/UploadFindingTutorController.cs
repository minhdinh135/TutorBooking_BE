using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Models.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadFindingTutorController : ControllerBase
    {
        private readonly IGenericService<User, TutorRequestDTO> _tutorRequestService;
        private readonly ILogger<UploadFindingTutorController> _logger;
        private readonly IConfiguration _configuration;

        public UploadFindingTutorController(IConfiguration config, ILogger<UploadFindingTutorController> logger,
                IGenericService<User, TutorRequestDTO> tutorRequestService)
        {
            _logger = logger;
            _configuration = config;
            _tutorRequestService = tutorRequestService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var tutorRequestList = await _tutorRequestService.GetAll();
            return Ok(tutorRequestList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var tutorRequest = await _tutorRequestService.Get(id);
            return Ok(tutorRequest);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(TutorRequestDTO dto)
        {
            var tutorRequest = await _tutorRequestService.Add(dto);
            return Ok(tutorRequest);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(TutorRequestDTO dto)
        {
            var tutorRequest = await _tutorRequestService.Update(dto);
            return Ok(tutorRequest);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var tutorRequest = await _tutorRequestService.Delete(id);
            return Ok(tutorRequest);
        }
    }
}
