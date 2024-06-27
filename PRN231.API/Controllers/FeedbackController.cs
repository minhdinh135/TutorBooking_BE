using PRN231.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Constant;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly IGenericService<Feedback, FeedbackDTO> _feedbackService;
        private readonly ILogger<FeedbackController> _logger;
        public IConfiguration _configuration;

        public FeedbackController(IConfiguration config, ILogger<FeedbackController> logger,
                IGenericService<Feedback, FeedbackDTO> feedbackService)
        {
            _logger = logger;
            _configuration = config;
            _feedbackService = feedbackService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var feedbackList = await _feedbackService.GetAll();
            return Ok(feedbackList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var feedbackList = await _feedbackService.Get(id);
            return Ok(feedbackList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(FeedbackDTO dto)
        {
            dto.Status = StatusConstant.PENDING;
            var feedback = await _feedbackService.Add(dto);
            return Ok(feedback);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(FeedbackDTO dto)
        {
            dto.Status = StatusConstant.PENDING;
            var feedback = await _feedbackService.Update(dto);
            return Ok(feedback);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var feedback = await _feedbackService.Delete(id);
            return Ok(feedback);
        }
    }
}
