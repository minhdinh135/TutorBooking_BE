using PRN231.Models;
using PRN231.Repositories.Interfaces;
using PRN231.Repository.Interfaces;

namespace PRN231.Repositories.Implementations
{
    public class SubjectLevelRepository : ISubjectLevelRepository
    {
        private readonly IGenericRepository<SubjectLevel> _genericRepository;

        public SubjectLevelRepository(IGenericRepository<SubjectLevel> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public SubjectLevel FindSubjectLevelBySubjectIdAndLevelId(int subjectId, int levelId)
        {
            return _genericRepository.GetAll().Result.FirstOrDefault(sl => sl.SubjectId == subjectId && sl.LevelId == levelId);
        }

        public Task<SubjectLevel> Add(SubjectLevel entity)
        {
            return _genericRepository.Add(entity);
        }

        public Task<SubjectLevel> Delete(params int[] keys)
        {
            return _genericRepository.Delete(keys);
        }

        public Task<SubjectLevel> Get(params int[] keys)
        {
            return _genericRepository.Get(keys);
        }

        public Task<IEnumerable<SubjectLevel>> GetAll(params Func<IQueryable<SubjectLevel>, IQueryable<SubjectLevel>>[] includes)
        {
            return _genericRepository.GetAll(includes);
        }

        public Task<SubjectLevel> Update(SubjectLevel entity)
        {
            return _genericRepository.Update(entity);
        }
    }
}
