using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Models.DTOs;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController:ControllerBase
    {
        private readonly IGenericService<Schedule, ScheduleDTO> _scheduleService;
        private readonly ILogger<ScheduleController> _logger;
        public IConfiguration _configuration;

        public ScheduleController(IConfiguration config, ILogger<ScheduleController> logger,
                IGenericService<Schedule, ScheduleDTO> scheduleService)
        {
            _logger = logger;
            _configuration = config;
            _scheduleService = scheduleService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var scheduleList = await _scheduleService.GetAll();
            return Ok(scheduleList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var scheduleList = await _scheduleService.Get(id);
            return Ok(scheduleList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(ScheduleDTO dto)
        {
            var schedule = await _scheduleService.Add(dto);
            return Ok(schedule);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(ScheduleDTO dto)
        {
            var schedule = await _scheduleService.Update(dto);
            return Ok(schedule);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var schedule = await _scheduleService.Delete(id);
            return Ok(schedule);
        }
    }
}
