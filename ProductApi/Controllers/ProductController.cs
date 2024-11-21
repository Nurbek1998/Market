using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductApi.Dtos.ProductDtos;
using ProductApi.Entities;
using ProductApi.Interfaces;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productService, ILogger<ProductsController> logger) : ControllerBase
    {
        /// <summary>
        /// Barcha mahsulotlarni olish
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts()
        {
            logger.LogInformation("The request is received");
            var products = await productService.GetAllProductsAsync();
            return Ok(products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                Status = p.Status
            }));
        }

        /// <summary>
        /// Id bo'yicha mahsulot olish
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProductById(Guid id)
        {
            logger.LogInformation("The request is received in GetProductById method");
            var product = await productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                ModifiedAt = product.ModifiedAt,
                Status = product.Status
            });
        }

        /// <summary>
        /// Yangi mahsulot yaratish
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> CreateProduct(ProductCreateDto dto)
        {
            logger.LogInformation("The request is received in CreateProduct method");
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Status = dto.Status
            };

            var createdProduct = await productService.CreateProductAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, new ProductReadDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                CreatedAt = createdProduct.CreatedAt,
                ModifiedAt = createdProduct.ModifiedAt,
                Status = createdProduct.Status
            });
        }

        /// <summary>
        /// Id bo'yicha mahsulotni yangilash
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReadDto>> UpdateProduct(Guid id, ProductUpdateDto dto)
        {
            logger.LogInformation("THe request is received in UpdateProduct");
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Status = dto.Status
            };

            var updatedProduct = await productService.UpdateProductAsync(id, product);
            if (updatedProduct == null) return NotFound();

            return Ok(new ProductReadDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                CreatedAt = updatedProduct.CreatedAt,
                ModifiedAt = updatedProduct.ModifiedAt,
                Status = updatedProduct.Status
            });
        }

        /// <summary>
        /// Id bo'yicha mahsulotni o'chirish
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            logger.LogInformation("The request is received in DeleteProduct method");
            var deleted = await productService.DeleteProductAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
