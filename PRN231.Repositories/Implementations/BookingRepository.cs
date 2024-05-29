using PRN231.DAL;
using PRN231.Models;
using PRN231.Repository.Implementations;

namespace PRN231.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>
    {
        public BookingRepository(SmartHeadContext dbContext) : base(dbContext)
        {
        }
    }
}
