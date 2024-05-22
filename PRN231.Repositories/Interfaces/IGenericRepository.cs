namespace EXE101.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(params Func<IQueryable<T>, IQueryable<T>>[] includes);
        Task<T> Get(params Guid[] keys);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(params Guid[] keys);
    }
}
