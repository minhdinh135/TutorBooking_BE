namespace EXE101.Models.DTOs
{
    public class FeedbackDTO
    {
        public Guid? Id { get; set; } 

        public required string Message { get; set; }

        public required Guid ProductId { get; set; }
    }
}
