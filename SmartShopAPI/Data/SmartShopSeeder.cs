using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Entities;
using SmartShopAPI.Models;

namespace SmartShopAPI.Data
{
    public class SmartShopSeeder
    {
        private readonly SmartShopDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public SmartShopSeeder(SmartShopDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _context = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            SeedCategories();
            SeedProducts();
            SeedRoles();
            SeedUsers();
        }

        private void SeedCategories()
        {
            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Oleje silnikowe" },
                    new Category { Name = "Klocki hamulcowe" },
                    new Category { Name = "Tarcze hamulcowe" },
                    new Category { Name = "Świece zapłonowe" },
                    new Category { Name = "Paski rozrządu" },
                    new Category { Name = "Opony" },
                    new Category { Name = "Amortyzatory" },
                    new Category { Name = "Akumulatory" },
                    new Category { Name = "Filtry olejowe" },
                    new Category { Name = "Części karoserii" }
                };
                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }

        private void SeedProducts()
        {
            if (!_context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Olej Mannol 5w40", 
                        Description = "Olej marki Mannol do samochodów osobowych", 
                        Price = 49.99M, StockQuantity = 10, CategoryId = 1 },
                    new Product { Name = "Olej Motul 5w30", 
                        Description = "Olej marki Motul do samochodów osobowych", 
                        Price = 59.99M, StockQuantity = 8, CategoryId = 1 },
                    new Product { Name = "Klocki hamulcowe Brembo", 
                        Description = "Klocki hamulcowe marki Brembo do samochodów sportowych", 
                        Price = 129.99M, StockQuantity = 15, CategoryId = 2 },
                    new Product { Name = "Tarcza hamulcowa ATE", 
                        Description = "Tarcza hamulcowa marki ATE o średnicy 280mm", 
                        Price = 199.99M, StockQuantity = 5, CategoryId = 2 },
                    new Product { Name = "Świeca zapłonowa NGK", 
                        Description = "Świeca zapłonowa marki NGK do silników benzynowych", 
                        Price = 14.99M, StockQuantity = 20, CategoryId = 3 },
                    new Product { Name = "Moduł zapłonowy Delphi", 
                        Description = "Moduł zapłonowy marki Delphi do silników z bezpośrednim wtryskiem", 
                        Price = 299.99M, StockQuantity = 7, CategoryId = 3 },
                    new Product { Name = "Tłumik końcowy Akrapovic", 
                        Description = "Tłumik końcowy marki Akrapovic do samochodów sportowych", 
                        Price = 1399.99M, StockQuantity = 3, CategoryId = 4 }
                };
                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
        }

        private void SeedRoles()
        {
            if (!_context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                };
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }
        }

        private void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var admin = new User
                {
                    Email = "admin@admin.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    RoleId = 1, 
                    Address = new Address
                    {
                        City = "Warsaw",
                        Street = "Admin St",
                        PostalCode = "00-111"
                    }
                };
                admin.PasswordHash = _passwordHasher.HashPassword(admin, "admin123");

                var user = new User
                {
                    Email = "user@user.com",
                    FirstName = "Regular",
                    LastName = "User",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    RoleId = 2,
                    Address = new Address
                    {
                        City = "Krakow",
                        Street = "User St",
                        PostalCode = "30-222"
                    }
                };
                user.PasswordHash = _passwordHasher.HashPassword(user, "user1234");
                _context.Users.AddRange(admin, user);
                _context.SaveChanges();
            }
        }
    }
}
