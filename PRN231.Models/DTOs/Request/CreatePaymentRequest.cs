namespace PRN231.Models.DTOs.Request
{
    public class CreatePaymentRequest
    {
        public long Amount { get; set; }
        public string OrderInfo { get; set; }
    }
}
