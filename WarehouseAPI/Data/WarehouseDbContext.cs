using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Models;

//сущность и контекст для работы с таблицей товаров.
namespace WarehouseAPI.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>()
                .HasIndex(p => new { p.CategoryId, p.Price });

            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category) //каждый товар связан с одной категорией
                .WithMany(c => c.Products) //у категории может быть много товаров
                .HasForeignKey(p => p.CategoryId) //внешний ключ находится в таблице товаров
                .IsRequired() //категория обязательна
                .OnDelete(DeleteBehavior.Restrict);
            //нельзя удалить категорию, пока на неё ссылаются товары

            modelBuilder.Entity<ProductModel>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Products_Price_NonNegative",
                        "\"Price\" >= 0");

                    t.HasCheckConstraint(
                        "CK_Products_Quantity_NonNegative",
                        "\"Quantity\" >= 0");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
