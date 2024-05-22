using EXE101.Models;

namespace EXE101.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithImages();

        Task<Product> GetWithAttributes(Guid id);

        Task<Product> GetWithImages(Guid id);
    }
}
