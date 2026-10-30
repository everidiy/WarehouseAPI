using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WarehouseAPI.DTOs;
using Xunit.Abstractions;

namespace WarehouseAPI_Tests
{
    public class ProductsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private readonly ITestOutputHelper _output;

        public ProductsIntegrationTests(WebApplicationFactory<Program> factory,
            ITestOutputHelper helper)
        {
            _client = factory.CreateClient();
            _output = helper;
        }

        [Fact]
        public async Task GETProducts_Returns200()
        {
            var response = await _client.GetAsync("/api/Products?search=Laptop&page=1&pageSize=5");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GETProduct6_Returns200()
        {
            var response = await _client.GetAsync("/api/Products/1");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GETProductCategoryId_Returns200()
        {
            var response = await _client.GetAsync("/api/Products?categoryId=2&minPrice=100&maxPrice=500");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POSTProduct5_Returns201()
        {
            var product = new CreatedProductDTO
            {
                Name = "Test Product",
                Price = 100,
                Quantity = 5,
                CategoryId = 1
            };

            var response = await _client.PostAsJsonAsync("/api/Products", product);

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PUTProduct6_Returns204()
        {
            var product = new UpdatedProductDTO
            {
                Name = "Updated Test Product",
                Price = 150,
                Quantity = 10
            };

            var response = await _client.PutAsJsonAsync("/api/Products/5", product);

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DELETEProduct6_Returns204()
        {
            var response = await _client.DeleteAsync("/api/Products/5");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
