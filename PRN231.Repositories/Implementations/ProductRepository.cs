using EXE101.Models;
using EXE101.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EXE101.Repository.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(LocoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithImages()
        {
            var products = await base.GetAll(p => p.Include(p => p.ProductImages),
                p => p.Include(p => p.Brand));
            /*foreach (var product in products)
            {
                foreach (var image in product.ProductImages)
                {
                    if(image == null) continue;
                    if (image.ImageUrl.Contains("localhost"))
                    {
                        var url = image.ImageUrl.Split("/").Last();
                        image.ImageUrl = "https://pig-measured-llama.ngrok-free.app/" + url;
                    }
                }
            }*/
            return products;
        }


        public async Task<Product> GetWithAttributes(Guid id)
        {
            var products = await base.GetAll(p => p.Include(p => p.ProductImages),
                p => p.Include(p => p.Brand),
                p => p.Include(p => p.VariantTypes)
                    .ThenInclude(vt => vt.Variants)
                    .ThenInclude(v => v.VariantProducts)
                    .ThenInclude(vp => vp.VariantFinalProduct));
            var product = products.FirstOrDefault(p => p.Id == id);
            
            return product;
        }


        public async Task<Product> GetWithImages(Guid id)
        {
            var products = await base.GetAll(p => p.Include(p => p.ProductImages));
            var product = products.FirstOrDefault(p => p.Id == id);
            /*foreach (var image in product.ProductImages)
            {
                if(image == null) continue;
                if (image.ImageUrl.Contains("localhost"))
                {
                    var url = image.ImageUrl.Split("/").Last();
                    image.ImageUrl = "https://pig-measured-llama.ngrok-free.app/" + url;
                }
            }*/
            return product;
        }
    }
}

