namespace PRN231.Models;

public partial class Credential
{
    public int Id { get; set; }

    public int TutorId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Image { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public User Tutor { get; set; }
}
