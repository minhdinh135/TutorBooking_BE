namespace PRN231.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int BookingId { get; set; }

    public decimal Price { get; set; }

    public string DayOfWeek { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public Service Service { get; set; }

    public Booking Booking { get; set; }
}
