namespace WarehouseAPI.Reports
{
    public class CategoryReportDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public int ProductCount { get; set; }

        public int TotalQuantity { get; set; }

        public decimal TotalValue { get; set; }
    }
}
