using SmartShopAPI.Models;

namespace SmartShopAPI.Data
{
    public class SmartShopSeeder
    {
        private readonly SmartShopDbContext _context;
        public SmartShopSeeder(SmartShopDbContext dbContext)
        {
            _context = dbContext;
        }

        public void Seed()
        {
            if(!_context.Categories.Any())
            {
                var result = GetCategories();
                _context.AddRange(result);
                _context.SaveChanges();
            }
            if (!_context.Products.Any())
            {
                var result = GetProducts();
                _context.AddRange(result);
                _context.SaveChanges();
            }
        }

        private IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>
            {
                new Category { Name = "Filtry oleju",
                               Products = new List<Product>() 
                               {
                                    new Product
                                    {
                                        Name = "Filtr olejowy Bosch",
                                        Description = "Filtr olejowy marki Bosch do samochodów osobowych",
                                        Price = 49.99M,
                                        StockQuantity = 10,
                                        CategoryId = 1
                                    },
                                    new Product
                                    {
                                        Name = "Filtr powietrza Mann-Filter",
                                        Description = "Filtr powietrza marki Mann-Filter do samochodów SUV",
                                        Price = 59.99M,
                                        StockQuantity = 8,
                                        CategoryId = 1
                                    },
                                }
                },
                new Category { Name = "Klocki hamulcowe" },
                new Category { Name = "Tarcze hamulcowe" },
                new Category { Name = "Świece zapłonowe" },
                new Category { Name = "Paski rozrządu" },
                new Category { Name = "Opony" },
                new Category { Name = "Amortyzatory" },
                new Category { Name = "Akumulatory" },
                new Category { Name = "Oleje silnikowe" },
                new Category { Name = "Części karoserii" }
            };

            return categories;
        }

        private IEnumerable<Product> GetProducts()
        {
            var products = new List<Product>
            {
                new Product { Name = "Filtr olejowy Bosch", Description = "Filtr olejowy marki Bosch do samochodów osobowych", Price = 49.99M, StockQuantity = 10, CategoryId = 1 },
                new Product { Name = "Filtr powietrza Mann-Filter", Description = "Filtr powietrza marki Mann-Filter do samochodów SUV", Price = 59.99M, StockQuantity = 8, CategoryId = 1 },

                new Product { Name = "Klocki hamulcowe Brembo", Description = "Klocki hamulcowe marki Brembo do samochodów sportowych", Price = 129.99M, StockQuantity = 15, CategoryId = 2 },
                new Product { Name = "Tarcza hamulcowa ATE", Description = "Tarcza hamulcowa marki ATE o średnicy 280mm", Price = 199.99M, StockQuantity = 5, CategoryId = 2 },

                new Product { Name = "Świeca zapłonowa NGK", Description = "Świeca zapłonowa marki NGK do silników benzynowych", Price = 14.99M, StockQuantity = 20, CategoryId = 3 },
                new Product { Name = "Moduł zapłonowy Delphi", Description = "Moduł zapłonowy marki Delphi do silników z bezpośrednim wtryskiem", Price = 299.99M, StockQuantity = 7, CategoryId = 3 },
                
                new Product { Name = "Tłumik końcowy Akrapovic", Description = "Tłumik końcowy marki Akrapovic do samochodów sportowych", Price = 1399.99M, StockQuantity = 3, CategoryId = 4 },
            };

            return products;
        }

    }
}
