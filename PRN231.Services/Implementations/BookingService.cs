using Microsoft.EntityFrameworkCore;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repositories.Interfaces;

namespace PRN231.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingUserRepository _bookingUserRepository;

        public BookingService(IBookingRepository bookingRepository,
            IBookingUserRepository bookingUserRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingUserRepository = bookingUserRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _bookingRepository.GetAll(
                    query => query.Include(b => b.BookingUsers)
                                  .Include(b => b.Schedules)
                );
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsByStatus(string status)
        {
            return GetAllBookings().Result.Where(b => b.Status.Equals(status));
        }

        public async Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            Booking booking = new Booking
            {
                SubjectId = createBookingRequest.SubjectId,
                LevelId = createBookingRequest.LevelId,
                Price = 0,
                PaymentMethod = PaymentMethodConstant.UNDEFINED,
                Description = createBookingRequest.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = BookingStatusConstant.PENDING
            };

            try
            {
                Booking addedBooking = await _bookingRepository.Add(booking);

                BookingUser bookingUser = new BookingUser
                {
                    UserId = createBookingRequest.UserId,
                    BookingId = addedBooking.Id,
                    Role = RoleEnum.STUDENT,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Status = StatusConstant.ACTIVE
                };

                BookingUser savedBookingUser = await _bookingUserRepository.Add(bookingUser);

                CreateBookingResponse bookingResponse = new CreateBookingResponse
                {
                    SubjectId = addedBooking.SubjectId,
                    LevelId = addedBooking.LevelId,
                    UserId = savedBookingUser.UserId,
                    Role = savedBookingUser.Role,
                    Description = addedBooking.Description
                };

                return bookingResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UpdateBookingResponse> UpdateBooking(UpdateBookingRequest updateBookingRequest)
        {
            try
            {
                Booking existingBooking = _bookingRepository.GetAll().Result
                    .FirstOrDefault(b => b.Id ==  updateBookingRequest.BookingId);
                existingBooking.SubjectId = updateBookingRequest.SubjectId;
                existingBooking.LevelId = updateBookingRequest.LevelId;
                existingBooking.Price = updateBookingRequest.Price;
                existingBooking.PaymentMethod = updateBookingRequest.PaymentMethod;
                existingBooking.Description = updateBookingRequest.Description;
                existingBooking.Status = updateBookingRequest.Status;

                Booking updatedBooking = await _bookingRepository.Update(existingBooking);

                UpdateBookingResponse bookingResponse = new UpdateBookingResponse
                {
                    SubjectId = updatedBooking.SubjectId,
                    LevelId = updatedBooking.LevelId,
                    Price = updatedBooking.Price,
                    PaymentMethod = updatedBooking.PaymentMethod,
                    Status = updatedBooking.Status
                };

                return bookingResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
