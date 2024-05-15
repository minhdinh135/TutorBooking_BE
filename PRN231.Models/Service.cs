namespace PRN231.Models;

public partial class Service
{
    public int Id { get; set; }

    public int TutorId { get; set; }

    public int SubjectId { get; set; }

    public string Name { get; set; }

    public string Content { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public User Tutor { get; set; }

    public Subject Subject { get; set; }

    public List<Order> Orders { get; set; }
}
