using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Services;
using System.Net;
using System.Runtime.InteropServices;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse>> GetAllBookings()
        {
            List<Booking> bookings = await _bookingService.GetAllBookings();

            var response = new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookings);

            return Ok(response);
        }

        [HttpGet("GetAllByStatus")]
        public async Task<ActionResult<ApiResponse>> GetAllBookingsByStatus([FromQuery] string status)
        {
            List<Booking> bookings = await _bookingService.GetAllBookingsByStatus(status);

            return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookings));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                CreateBookingResponse bookingResponse = await _bookingService.CreateBooking(request);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse));
            } catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult<ApiResponse>> UpdateBooking([FromBody] UpdateBookingRequest request)
        {
            try
            {
                UpdateBookingResponse bookingResponse = await _bookingService.UpdateBooking(request);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }
    }
}