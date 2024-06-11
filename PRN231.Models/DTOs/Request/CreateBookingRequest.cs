namespace PRN231.Models.DTOs.Request
{
    public class CreateBookingRequest
    {
        public int SubjectId { get; set; }

        public int LevelId { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }
       
    }
}
