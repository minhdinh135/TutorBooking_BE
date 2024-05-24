using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;
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
    }
}
