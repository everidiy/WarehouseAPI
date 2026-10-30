using WarehouseAPI.DTOs;
using WarehouseAPI.Validators;
using Xunit.Abstractions;

namespace WarehouseAPI_Tests
{
    public class ProductValidatorTests
    {
        private readonly ITestOutputHelper _output;

        public ProductValidatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void IsProductValid(CreatedProductDTO model, bool expected)
        {
            var validator = new ProductValidator();

            bool actual = validator.IsProductValid(model);

            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> ProductTestData()
        {
            yield return new object[]
            {
                new CreatedProductDTO
                {
                    Name = "Ноутбук",
                    Price = 100,
                    Quantity = 10
                },
                true
            };

            yield return new object[]
            {
                new CreatedProductDTO
                {
                    Name = " ",
                    Price = -100,
                    Quantity = 0
                },
                false
            };

            yield return new object[]
            {
                new CreatedProductDTO
                {
                    Name = "Ноутбук",
                    Price = 100,
                    Quantity = -1
                },
                false
            };
            yield return new object[]
            {
                null,
                false
            };
        }
    }
}
