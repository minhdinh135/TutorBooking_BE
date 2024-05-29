using PRN231.Models;

namespace PRN231.Models.DTOs
{
    public class FeedbackDTO
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int StudentId { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public bool Status { get; set; }
    }
}
