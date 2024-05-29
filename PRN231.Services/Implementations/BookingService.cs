using AutoMapper;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repositories.Implementations;
using PRN231.Repository.Interfaces;
using PRN231.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepository;

        public BookingService(IGenericRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<Booking>> GetAllBookings()
        {
            return (List<Booking>)await _bookingRepository.GetAll();
        }

        public async Task<CreateBookingResponse> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            Booking booking = new Booking
            {
                StudentId = createBookingRequest.StudentId,
                Price = createBookingRequest.Price,
                PaymentMethod = createBookingRequest.PaymentMethod,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Status = BookingStatusConstant.PENDING
            };

            try
            {
                Booking addedBooking = await _bookingRepository.Add(booking);

                CreateBookingResponse bookingResponse = new CreateBookingResponse
                {
                    StudentId = addedBooking.StudentId,
                    Price = addedBooking.Price,
                    PaymentMethod = addedBooking.PaymentMethod
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
                Booking existingBooking = _bookingRepository.GetAll().Result.FirstOrDefault(b => b.Id ==  updateBookingRequest.BookingId);
                existingBooking.Price = updateBookingRequest.Price;
                existingBooking.PaymentMethod = updateBookingRequest.PaymentMethod;
                existingBooking.Status = updateBookingRequest.Status;

                Booking updatedBooking = await _bookingRepository.Update(existingBooking);

                UpdateBookingResponse bookingResponse = new UpdateBookingResponse
                {
                    StudentId = updatedBooking.StudentId,
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
