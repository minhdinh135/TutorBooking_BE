namespace PRN231.Models.DTOs.Response
{
    public class UpdateBookingResponse
    {
        public int SubjectId { get; set; }
        public int LevelId { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
    }
}
