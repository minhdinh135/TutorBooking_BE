using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs.Request
{
    public class CreateBookingRequest
    {
        public int SubjectLevelId { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
    }
}
