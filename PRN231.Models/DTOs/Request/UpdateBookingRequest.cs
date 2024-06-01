namespace PRN231.Models.DTOs.Request
{
    public class UpdateBookingRequest
    {
        public int BookingId { get; set; }
        public int SubjectLevelId { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
