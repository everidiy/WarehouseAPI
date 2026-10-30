using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly WarehouseDbContext _context;

        public ProductsController(WarehouseDbContext context)
        {
            _context = context;
        }

        //CRUD products

        //READ ALL
        //GET: api/products?search=.&page=.&pageSize=.
        [HttpGet]
        public async Task<ActionResult> GetProducts(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(
                    "Номер страницы должен быть больше 0, " +
                    "а размер страницы — от 1 до 100.");
            }

            var query = _context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(P => 
                    EF.Functions.ILike(P.Name, $"%{search.Trim()}%"));
            }

            var total = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ResponseProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToListAsync();

            return Ok(new
            {
                page, 
                pageSize, 
                total, 
                totalPages = (int)Math.Ceiling(
                    total / (double)pageSize),
                items = products
            });
        }

        //READ ONE
        //GET: api/products/int(5)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var dto = new ResponseProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };

            return Ok(dto);
        }

        //GET PRODUCTS BY categoryId and minPrice/maxPrice
        //GET: api/products?categoryId=2&minPrice=100&maxPrice=500
        [HttpGet("filter")]
        public async Task<ActionResult> GetProductsListByCategoryIdAndMinMaxPrices(
            [FromQuery] int? categoryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            if (minPrice.HasValue && maxPrice.HasValue &&
                minPrice.Value > maxPrice.Value)
            {
                return BadRequest("Минимальная цена не может быть больше максимальной.");
            }

            var query = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                var categoryExists = await _context.Categories.AnyAsync(
                    c => c.Id == categoryId.Value);

                if (!categoryExists)
                {
                    return BadRequest("Такой категории не существует.");
                }

                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            var total = await query.CountAsync();

            var products = await query
                    .OrderBy(p => p.CategoryId)
                    .Select(p => new ResponseProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        CategoryId = p.CategoryId
                    }).ToListAsync();

            return Ok(new
            {
                total,
                items = products
            });
        }

        //CREATE
        //POST: api/products
        [HttpPost]
        public async Task<ActionResult<ResponseProductDTO>> CreateProduct(CreatedProductDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Название товара обязательно.");
            }

            if (dto.Price < 0)
            {
                return BadRequest("Цена не может быть отрицательной.");
            }

            if (dto.Quantity < 0)
            {
                return BadRequest("Количество не может быть отрицательным.");
            }

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
            {
                return BadRequest("Такой категории не существует.");
            }

            var product = new ProductModel
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId
            };

            await _context.SaveChangesAsync();

            var dtoRes = new ResponseProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };

            return CreatedAtAction(
                nameof(GetProduct),
                new { id = dtoRes.Id },
                dtoRes);
        }

        //UPDATE
        //PUT: api/products/int(5)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdatedProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE
        //DELETE: api/products/int(5)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
