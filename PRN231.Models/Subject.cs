namespace PRN231.Models;

public partial class Subject : BaseModel
{
    public int? CredentialId { get; set; }

    public string Name { get; set; }

    public List<SubjectLevel> SubjectLevels{ get; set; }

    public Credential? Credential { get; set; }
}
