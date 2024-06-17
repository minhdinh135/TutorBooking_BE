namespace PRN231.Models;

public partial class Schedule : BaseModel
{
    public int BookingId { get; set; }

    public string DayOfWeek { get; set; }

    public DateTime StartTime { get; set; }

    public string Period { get; set; }

    public int DurationInSeconds { get; set; }

    public Booking Booking { get; set; }
}
