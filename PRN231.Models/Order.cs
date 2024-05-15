namespace PRN231.Models;

public partial class Order
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int StudentId { get; set; }

    public decimal Price { get; set; }

    public string PaymentMethod { get; set; }

    public int Rating { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public User Student { get; set; }

    public Service Service { get; set; }
}
