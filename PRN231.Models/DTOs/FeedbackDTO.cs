using PRN231.Models;

namespace EXE101.Models.DTOs
{
    public class FeedbackDTO
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int StudentId { get; set; }

        public string Content { get; set; }

        public int Rating { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool Status { get; set; }

        public User Tutor { get; set; }

        public User Student { get; set; }
    }
}
