using AutoMapper;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repositories.Implementations;
using PRN231.Repository.Interfaces;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Services.Implementation
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
    }
}
