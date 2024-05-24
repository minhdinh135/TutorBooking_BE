using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EXE101.Models;
using EXE101.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;

namespace EXE101.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInfoController : ControllerBase
    {
        private readonly IGenericService<User, UserInfoDTO> _userInfoService;
        private readonly ILogger<UserInfoController> _logger;
        public IConfiguration _configuration;

        public UserInfoController(IConfiguration config, ILogger<UserInfoController> logger,
                IGenericService<User, UserInfoDTO> userInfoService)
        {
            _logger = logger;
            _configuration = config;
            _userInfoService = userInfoService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll(){
            var customerList = await _userInfoService.GetAll();
            return Ok(customerList);
        }

        [HttpGet("GetAllWithUserId")]
        //[Authorize]
        public async Task<IActionResult> GetAllWithUserId(Guid userId){
            var customerList = await _userInfoService.GetAll();
            //customerList = customerList.Where(x => x.UserId == userId).ToList();
            return Ok(customerList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(Guid id){
            var customerList = await _userInfoService.Get(id);
            return Ok(customerList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(UserInfoDTO dto){
            var brand = await _userInfoService.Add(dto);
            return Ok(brand);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(UserInfoDTO dto){
            var brand = await _userInfoService.Update(dto);
            return Ok(brand);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody]Guid id){
            var brand = await _userInfoService.Delete(id);
            return Ok(brand);
        }
    }
}
