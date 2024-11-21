using ProductApi.Entities;

namespace ProductApi.Dtos.ProductDtos
{
    public class ProductCreateDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public EProductStatus Status { get; set; }
    }
}
