using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Services;
using System.Net;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController
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

            return response;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                CreateBookingResponse bookingResponse = await _bookingService.CreateBooking(request);

                return new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse);
            } catch (Exception ex)
            {
                return new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null);
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult<ApiResponse>> UpdateBooking([FromBody] UpdateBookingRequest request)
        {
            try
            {
                UpdateBookingResponse bookingResponse = await _bookingService.UpdateBooking(request);

                return new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null);
            }
        }
    }
}