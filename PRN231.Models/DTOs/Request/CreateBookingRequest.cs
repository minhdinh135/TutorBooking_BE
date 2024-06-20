using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace PRN231.Models.DTOs.Request
{
    public class CreateBookingRequest
    {
        public int SubjectId { get; set; }

        public int LevelId { get; set; }

        public int UserId { get; set; }

        public int NumOfSlots { get; set; }

        //public IEnumerable<ScheduleDTO>? Schedules { get; set; }
        public decimal PricePerSlot { get; set; }
        public string Description { get; set; }
       
    }
}
