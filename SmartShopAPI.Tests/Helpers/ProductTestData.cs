using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;
using System.Diagnostics.Contracts;

namespace SmartShopAPI.Tests.Helpers
{
    public class ProductTestData
    {
        public static List<Category> Categories => new()
            {
                new Category { Id = 1, Name = "Electronics" },
            };

        public static List<Product> Products => new List<Product>
        {
            new Product { Name = "Product1", Description = "Description for Product1", Price = 10.00M, StockQuantity = 100, CategoryId = 1 },
            new Product { Name = "Product2", Description = "Description for Product2", Price = 20.00M, StockQuantity = 150, CategoryId = 1 },
            new Product { Name = "Product3", Description = "Description for Product3", Price = 30.00M, StockQuantity = 200, CategoryId = 1 },
            new Product { Name = "Product4", Description = "Description for Product4", Price = 40.00M, StockQuantity = 250, CategoryId = 1 }
        };

        public static QueryParams QueryParams => 
            new ()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchPhrase = "op",
                SortBy = "Price",
                SortOrder = SortOrder.Ascending
            };

        public static IEnumerable<object[]> InvalidQueryParamsList => 
            new List<object[]>
            {
                new object[] { new QueryParams
                {
                    PageNumber = 1,
                    PageSize = 10,
                    SearchPhrase = "op",
                    SortBy = "xxx",
                    SortOrder = SortOrder.Ascending
                } },
                new object[] { new QueryParams
                {
                    PageNumber = -1,
                    PageSize = 10,
                    SearchPhrase = "op",
                    SortBy = "Price",
                    SortOrder = SortOrder.Ascending
                } },
                new object[] { new QueryParams
                {
                    PageNumber = 1,
                    PageSize = -10,
                    SearchPhrase = "op",
                    SortBy = "Price",
                    SortOrder = SortOrder.Ascending
                } },
                new object[] { new QueryParams
                {
                    PageNumber = 1,
                    PageSize = 1,
                    SearchPhrase = "op",
                    SortBy = "Price",
                    SortOrder = SortOrder.Descending
                } }
            };

        public static IEnumerable<object[]> InvalidProductDataList =>
            new List<object[]>
            {
                new object[] { new CreateProductDto { Name = "Oil Castrol", Price = 0 , Description = "5w30"} },
                new object[] { new CreateProductDto { Name = "Oil Motul", Price = -1, Description = "5w50" } },
                new object[] { new CreateProductDto { Name = "", Price = 10, Description = "5w40" } },
            };

        public static CreateProductDto CreateProductDto => 
            new ()
            { 
                Name = "Motul Oil", 
                Description = "15w30", 
                Price = 150
            };

        public static UpdateProductDto UpdateProductDto =>
            new ()
            {
                Name = "Updated product",
                Description = "Updated Description",
                Price = 88,
                StockQuantity = 88
            };

        public static UpdateProductDto InvalidUpdateProductDto => new () { Name = "" };
    }
}
