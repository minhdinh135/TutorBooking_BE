namespace PRN231.Models;

public partial class SubjectLevel : BaseModel
{
    public int SubjectId { get; set; }

    public int LevelId { get; set; }

    public string? Description { get; set; }

    public Subject Subject { get; set; }

    public Level Level { get; set; }

    public List<Booking> Bookings { get; set; }

}
