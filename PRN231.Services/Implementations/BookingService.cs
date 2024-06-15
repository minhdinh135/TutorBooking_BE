using Microsoft.EntityFrameworkCore;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
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
                    query => query.Include(b => b.Subject)
                                  .Include(b => b.Level)
                                  .Include(b => b.BookingUsers)
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

        public async Task<IEnumerable<BookingUserDTO>> GetAllTutorsByBooking(int bookingId)
        {
            var bookingUsers = await _bookingUserRepository.GetAll(
                query => query.Where(bu => bu.BookingId == bookingId && bu.Role == RoleEnum.TUTOR)
            );

            return bookingUsers.Select(bu => new BookingUserDTO
            {
                UserId = bu.UserId,
                BookingId = bu.BookingId,
                Role = bu.Role,
                Status = bu.Status,
                Description = bu.Description
            });
        }

        public async Task<bool> ApplyToBooking(int userId, int bookingId)
        {
            BookingUser bookingUser = new BookingUser
            {
                UserId = userId,
                BookingId = bookingId,
                Role = RoleEnum.TUTOR,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = BookingUserStatusConstant.PENDING
            };

            try
            {
                await _bookingUserRepository.Add(bookingUser);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AcceptTutor(int bookingId, int tutorId)
        {
            var bookingUsers = await _bookingUserRepository.GetAll(
                query => query.Where(bu => bu.BookingId == bookingId && bu.Role == RoleEnum.TUTOR)
            );

            foreach (var bu in bookingUsers)
            {
                if (bu.UserId == tutorId)
                {
                    bu.Status = BookingUserStatusConstant.APPROVED;
                }
                else
                {
                    bu.Status = BookingUserStatusConstant.REJECTED;
                }

                await _bookingUserRepository.Update(bu);
            }

            return true;
        }
    }
}
