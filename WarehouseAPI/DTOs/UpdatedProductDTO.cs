namespace WarehouseAPI.DTOs
{
    public class UpdatedProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
