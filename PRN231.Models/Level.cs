namespace PRN231.Models;

public partial class Level : BaseModel
{
    public string LevelName { get; set; }

    public List<SubjectLevel> SubjectLevels { get; set; }
}
