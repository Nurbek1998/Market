using Microsoft.EntityFrameworkCore;
using ProductApi.Entities;
using ProductApi.Interfaces;

namespace ProductApi.Services
{
    public class ProductDetailService(AppDbContext dbContext, ILogger<ProductDetailService> logger) : IProductDetailService
    {
        public async Task<ProductDetail?> GetDetailsByProductIdAsync(Guid productId)
        {
            var productDetail = await dbContext.ProductDetails.FirstOrDefaultAsync(d => d.ProductId == productId);
            if (productDetail == null)
            {
                logger.LogInformation("The data is getting by Id from database");
                return default;
            }
            return productDetail;
        }

        public async Task<ProductDetail> CreateProductDetailAsync(Guid productId, ProductDetail detail)
        {
            detail.Id = Guid.NewGuid();
            detail.ProductId = productId;

            dbContext.ProductDetails.Add(detail);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("The data is creating in database");
            return detail;
        }

        public async Task<ProductDetail?> UpdateProductDetailAsync(Guid productId, ProductDetail detail)
        {
            var existingDetail = await dbContext.ProductDetails.FirstOrDefaultAsync(d => d.ProductId == productId);
            if (existingDetail == null) return null;

            existingDetail.Description = detail.Description;
            existingDetail.Color = detail.Color;
            existingDetail.Material = detail.Material;
            existingDetail.Weight = detail.Weight;
            existingDetail.QuantityInStock = detail.QuantityInStock;
            existingDetail.ManufactureDate = detail.ManufactureDate;
            existingDetail.ExpiryDate = detail.ExpiryDate;
            existingDetail.Size = detail.Size;
            existingDetail.Manufacturer = detail.Manufacturer;
            existingDetail.CountryOfOrigin = detail.CountryOfOrigin;

            await dbContext.SaveChangesAsync();
            logger.LogInformation("The data is updating from database");
            return existingDetail;
        }

        public async Task<bool> DeleteProductDetailAsync(Guid productId)
        {
            var detail = await dbContext.ProductDetails.FirstOrDefaultAsync(d => d.ProductId == productId);
            if (detail == null) return false;

            dbContext.ProductDetails.Remove(detail);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("The data is deleting from database");
            return true;
        }
    }
}
