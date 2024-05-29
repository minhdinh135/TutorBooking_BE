using Microsoft.EntityFrameworkCore;
using PRN231.DAL;
using PRN231.Models;
using PRN231.Repositories.Interfaces;
using PRN231.Repository.Implementations;
using PRN231.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(SmartHeadContext dbContext) : base(dbContext)
        {
        }
    }
}
