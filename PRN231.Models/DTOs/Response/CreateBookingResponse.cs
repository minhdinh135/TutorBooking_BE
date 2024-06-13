namespace PRN231.Models.DTOs.Response
{
    public class CreateBookingResponse
    {
        public int SubjectId { get; set; }

        public int LevelId { get; set; }

        public int UserId { get; set; }

        public string Role { get; set; }

        public string Description { get; set; }
    }
}
