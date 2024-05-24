using EXE101.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly ILogger<UserController> _logger;
        public IConfiguration _configuration;

        public UserController(IConfiguration config, ILogger<UserController> logger,
                IGenericService<User, UserDTO> userService)
        {
            _logger = logger;
            _configuration = config;
            _userService = userService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var customerList = await _userService.GetAll();
            return Ok(customerList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var customerList = await _userService.Get(id);
            return Ok(customerList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(UserDTO dto)
        {
            var brand = await _userService.Add(dto);
            return Ok(brand);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(UserDTO dto)
        {
            var brand = await _userService.Update(dto);
            return Ok(brand);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var brand = await _userService.Delete(id);
            return Ok(brand);
        }

    }
}
