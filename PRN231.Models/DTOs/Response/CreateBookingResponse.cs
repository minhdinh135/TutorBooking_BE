namespace PRN231.Models.DTOs.Response
{
    public class CreateBookingResponse
    {
        public int StudentId { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
    }
}
