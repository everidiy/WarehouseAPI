using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using WarehouseAPI.DTOs;
using Xunit.Abstractions;

namespace WarehouseAPI_Tests
{
    public class CategoryIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private readonly ITestOutputHelper _output;

        public CategoryIntegrationTests(WebApplicationFactory<Program> factory,
            ITestOutputHelper helper)
        {
            _client = factory.CreateClient();
            _output = helper;
        }

        [Fact]
        public async Task GETCategoriesProducts_Returns200()
        {
            var response = await _client.GetAsync("/api/Category?search=Electronics&page=1&pageSize=10");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GETCategory1_Returns200()
        {
            var response = await _client.GetAsync("/api/Category/1");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task POSTCategory_Returns201()
        {
            var product = new CreatedCategoryDTO
            {
                Name = "Audio & Media"
            };

            var response = await _client.PostAsJsonAsync("/api/Category", product);

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PUTCategory1_Returns204()
        {
            var product = new UpdatedCategoryDTO
            {
                Name = "Updated Audio & Media",
            };

            var response = await _client.PutAsJsonAsync("/api/Category/1", product);

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DELETECategory1_Returns204()
        {
            var response = await _client.DeleteAsync("/api/Category/3");

            var body = await response.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {(int)response.StatusCode}");
            _output.WriteLine(body);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
