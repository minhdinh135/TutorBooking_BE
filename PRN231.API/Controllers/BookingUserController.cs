using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingUserController : ControllerBase
    {
        private readonly IGenericService<BookingUser, BookingUserDTO> _bookingUserService;
        private readonly IGenericRepository<BookingUser> _bookingUserRepo;
        private readonly ILogger<BookingUserController> _logger;

        public BookingUserController(ILogger<BookingUserController> logger,
                IGenericRepository<BookingUser> bookingUserRepo,
                IGenericService<BookingUser, BookingUserDTO> bookingUserService)
        {
            _logger = logger;
            _bookingUserRepo = bookingUserRepo;
            _bookingUserService = bookingUserService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var bookingUserList = await _bookingUserRepo.GetAll();
            return Ok(bookingUserList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var bookingUser = await _bookingUserService.Get(id);
            return Ok(bookingUser);
        }

        [HttpGet("GetBookingUserByUserId")]
        //[Authorize]
        public async Task<IActionResult> GetBookingUserByUserId(int id)
        {
            var bookingUserList = await _bookingUserRepo.GetAll(x => x.Include(a => a.User));
            bookingUserList = bookingUserList.Where(c => c.UserId == id);
            return Ok(bookingUserList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(BookingUserDTO dto)
        {
            var bookingUser = await _bookingUserService.Add(dto);
            return Ok(bookingUser);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(BookingUserDTO dto)
        {
            var bookingUser = await _bookingUserService.Update(dto);
            return Ok(bookingUser);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var bookingUser = await _bookingUserService.Delete(id);
            return Ok(bookingUser);
        }
    }
}
