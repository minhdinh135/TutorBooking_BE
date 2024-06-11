using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;

namespace PRN231.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<IEnumerable<Booking>> GetAllBookingsByStatus(string status);
        Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest);
        Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest);
    }
}
