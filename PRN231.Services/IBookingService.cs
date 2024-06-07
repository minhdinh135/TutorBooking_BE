using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;

namespace PRN231.Services
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookings();
        Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest);
        Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest);
    }
}
