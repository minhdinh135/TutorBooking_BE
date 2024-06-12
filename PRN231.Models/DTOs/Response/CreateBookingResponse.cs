namespace PRN231.Models.DTOs.Response
{
    public class CreateBookingResponse
    {
        public int SubjectLevelId { get; set; }

        public int UserId { get; set; }

        public string Role { get; set; }

        public string Description { get; set; }
    }
}
