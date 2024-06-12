namespace PRN231.Models;

public partial class Booking : BaseModel
{
    public int SubjectLevelId { get; set; }

    public decimal Price { get; set; }

    public string PaymentMethod { get; set; }

    public string? Description { get; set; }

    public SubjectLevel SubjectLevel { get; set; }

    public List<Schedule> Schedules { get; set; }

    public List<BookingUser> BookingUsers { get; set; }

}
