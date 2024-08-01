using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Tests.Helpers
{
    public class ProductTestData
    {
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
