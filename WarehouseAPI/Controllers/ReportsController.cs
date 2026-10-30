using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WarehouseAPI.Reports;

namespace WarehouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategoryReportDto>>> GetCategoriesReport()
        {
            const string sql = """
                SELECT
                    c."Id" AS "CategoryId",
                    c."Name" AS "CategoryName",
                    COUNT(p."Id")::int AS "ProductCount",
                    COALESCE(SUM(p."Quantity"), 0)::int AS "TotalQuantity",
                    COALESCE(SUM(p."Price" * p."Quantity"), 0) AS "TotalValue"
                FROM "Categories" AS c
                LEFT JOIN "Products" AS p
                    ON p."CategoryId" = c."Id"
                GROUP BY c."Id", c."Name"
                ORDER BY c."Name";
                """;

            var connectionStr = _configuration.GetConnectionString("DefaultConnection");

            await using var connection = new NpgsqlConnection(connectionStr);

            var report = await connection.QueryAsync<CategoryReportDto>(sql);

            return Ok(report);
        }
    }
}
