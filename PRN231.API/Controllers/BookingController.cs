using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Response;
using PRN231.Services;
using PRN231.Services.Implementation;
using PRN231.Services.Interfaces;
using System.Net;
using System.Reflection.Metadata.Ecma335;

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

            var response = new ApiResponse((int)HttpStatusCode.OK, "Get successfully", bookings);

            return response;
        }
    }
}