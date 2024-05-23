using Microsoft.AspNetCore.Identity;

namespace PRN231.Models;

public partial class User : IdentityUser<int>
{
    /*public string Name { get; set; }

    public string Email {  get; set; }

    public string Password { get; set; }
    public string Phone { get; set; }*/

    public string Address { get; set; }

    public string Avatar { get; set; }

    public string Gender { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool Status { get; set; }

    public List<Credential>? Credentials { get; set; }

    public List<Service>? Services { get; set; }

    public List<Feedback>? StudentFeedbacks { get; set; }

    public List<Feedback>? TutorFeedbacks { get; set; }

    public List<Booking>? Bookings { get; set; }

}
