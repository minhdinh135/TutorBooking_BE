using EXE101.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialController : ControllerBase //tuan
    {
        private readonly IGenericService<Credential, CredentialDTO> _credentialService;
        private readonly ILogger<CredentialController> _logger;
        public IConfiguration _configuration;

        public CredentialController(IConfiguration config, ILogger<CredentialController> logger,
                IGenericService<Credential, CredentialDTO> credentialService)
        {
            _logger = logger;
            _configuration = config;
            _credentialService = credentialService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var credentialList = await _credentialService.GetAll();
            return Ok(credentialList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var credentialList = await _credentialService.Get(id);
            return Ok(credentialList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(CredentialDTO dto)
        {
            var credential = await _credentialService.Add(dto);
            return Ok(credential);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(CredentialDTO dto)
        {
            var credential = await _credentialService.Update(dto);
            return Ok(credential);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var credential = await _credentialService.Delete(id);
            return Ok(credential);
        }
    }
}
