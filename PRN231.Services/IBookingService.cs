using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookings();
        Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest);
        Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest);
    }
}
