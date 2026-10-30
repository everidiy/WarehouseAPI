namespace WarehouseAPI.DTOs
{
    public class ResponseProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int CategoryId { get; set; }
    }
}
