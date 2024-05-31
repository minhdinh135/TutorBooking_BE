namespace PRN231.Models;

public partial class Subject : BaseModel
{
    public string Name { get; set; }

    public List<SubjectLevel> SubjectLevels{ get; set; }

    public List<Credential> Credentials { get; set; }
}
