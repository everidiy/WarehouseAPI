namespace WarehouseAPI.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<ProductModel> Products { get; set; }
            = new List<ProductModel>();
    }
}
