using SmartShopAPI.Entities;
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
            if (!_context.Roles.Any())
            {
                var result = GetRoles();
                _context.AddRange(result);
                _context.SaveChanges();
            }
            if(!_context.Users.Any())
            {
                var result = GetUsers();
                _context.AddRange(result);
                _context.SaveChanges();
            }
        }
        public List<User> GetUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Email = "john.doe@example.com",
                    PasswordHash = "hashed_password_1",
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    RoleId = 2,
                    Address = new Address
                    {
                        City = "Warsaw",
                        Street = "Main St",
                        PostalCode = "00-123"
                    }
                },
                new User
                {
                    Email = "jane.smith@example.com",
                    PasswordHash = "hashed_password_2",
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(1985, 8, 25),
                    RoleId = 3,
                    Address = new Address
                    {
                        City = "Krakow",
                        Street = "Second St",
                        PostalCode = "30-456"
                    }
                },
                new User
                {
                    Email = "alice.johnson@example.com",
                    PasswordHash = "hashed_password_3",
                    FirstName = "Alice",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(1988, 3, 10),
                    RoleId = 1,
                    Address = new Address
                    {
                        City = "Poznan",
                        Street = "Third St",
                        PostalCode = "61-789"
                    }
                },
                new User
                {
                    Email = "robert.wilson@example.com",
                    PasswordHash = "hashed_password_4",
                    FirstName = "Robert",
                    LastName = "Wilson",
                    DateOfBirth = new DateTime(1995, 11, 8),
                    RoleId = 3,
                    Address = new Address
                    {
                        City = "Wroclaw",
                        Street = "Fourth St",
                        PostalCode = "50-234"
                    }
                },
                new User
                {
                    Email = "emily.brown@example.com",
                    PasswordHash = "hashed_password_5",
                    FirstName = "Emily",
                    LastName = "Brown",
                    DateOfBirth = new DateTime(1980, 7, 20),
                    RoleId = 2,
                    Address = new Address
                    {
                        City = "Lodz",
                        Street = "Fifth St",
                        PostalCode = "90-567"
                    }
                }
            };
            return users;
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Name = "Admin"
                },
                new Role
                {
                    Name = "Manager"
                },
                new Role
                {
                    Name = "User"
                }
            };
            return roles;
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
