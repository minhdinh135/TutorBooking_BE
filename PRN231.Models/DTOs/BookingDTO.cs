using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public decimal Price { get; set; }

        public string PaymentMethod { get; set; }

        public bool Status { get; set; }

    }
}
