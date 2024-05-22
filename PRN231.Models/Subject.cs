namespace PRN231.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Level { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public List<Service> Services { get; set; }
}
