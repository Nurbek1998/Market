using Microsoft.EntityFrameworkCore;
using ProductApi.Entities;
using ProductApi.Interfaces;

namespace ProductApi.Services
{
    public class ProductService(AppDbContext dbContext, ILogger<ProductService> logger) : IProductService
    {
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            logger.LogInformation("The informations are getting from database");
            return await dbContext.Products.Include(p => p.ProductDetail).ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            logger.LogInformation("The data is getting by Id from database");
            var product = await dbContext.Products.Include(p => p.ProductDetail).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
            {
                logger.LogInformation("The data is not exist");
                return default;
            }
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {

            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;
            product.ModifiedAt = DateTime.UtcNow;

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("The data is creating in database");
            return product;
        }

        public async Task<Product?> UpdateProductAsync(Guid id, Product product)
        {
            var existingProduct = await dbContext.Products.FindAsync(id);
            if (existingProduct == null) return null;

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.ModifiedAt = DateTime.UtcNow;
            existingProduct.Status = product.Status;

            await dbContext.SaveChangesAsync();
            logger.LogInformation("The data is updating from database");
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);
            if (product == null) return false;

            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("The data is deleting from database");
            return true;
        }
    }
}
