namespace PRN231.Models;

public partial class Feedback
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
