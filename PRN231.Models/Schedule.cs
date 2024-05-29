namespace PRN231.Models;

public partial class Schedule : BaseModel
{
    public int ServiceId { get; set; }

    public int BookingId { get; set; }

    public decimal Price { get; set; }

    public string DayOfWeek { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public Booking Booking { get; set; }
}
