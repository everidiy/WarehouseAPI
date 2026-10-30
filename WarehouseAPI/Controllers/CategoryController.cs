using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

namespace WarehouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly WarehouseDbContext _context;

        public CategoryController(WarehouseDbContext context)
        {
            _context = context;
        }

        //CRUD category

        //READ ALL
        //GET: api/categories?search=.&page=.&pageSize=.
        [HttpGet]
        public async Task<ActionResult> GetCategories(
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

            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    EF.Functions.ILike(c.Name, $"%{search.Trim()}%"));
            }

            var total = await query.CountAsync();

            var categories = await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                total,
                totalPages = (int)Math.Ceiling(
                    total / (double)pageSize),
                items = categories
            });
        }

        //READ ONE
        //GET: api/categories/int(5)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var dto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(dto);
        }

        //CREATE
        //POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory(CreatedCategoryDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return BadRequest("Некорректные данные категории");
            }

            var category = new CategoryModel
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            var dtoRes = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = dtoRes.Id },
                dtoRes);
        }

        //UPDATE
        //PUT: api/categories/int(5)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdatedCategoryDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = dto.Name;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE
        //DELETE: api/categories/int(5)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
