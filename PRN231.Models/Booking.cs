namespace PRN231.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public decimal Price { get; set; }

    public string PaymentMethod { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public User Student { get; set; }

    public List<Schedule> Schedules { get; set; }
}
