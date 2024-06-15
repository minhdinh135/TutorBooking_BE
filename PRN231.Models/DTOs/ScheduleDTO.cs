using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public string DayOfWeek { get; set; }

        public string Period { get; set; }

        public int DurationInSeconds { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; }
    }
}