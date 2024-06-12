using PRN231.Models;
using PRN231.Repository.Interfaces;

namespace PRN231.Repositories.Interfaces
{
    public interface ISubjectLevelRepository : IGenericRepository<SubjectLevel>
    {
        SubjectLevel FindSubjectLevelBySubjectIdAndLevelId(int subjectId, int levelId);
    }
}
