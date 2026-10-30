using WarehouseAPI.Models;

namespace WarehouseAPI.DTOs
{
    public class CreatedProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }

        public CategoryModel? Category { get; set; }
    }
}
