using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Data;
using SmartShopAPI.Entities;
using SmartShopAPI.Services;

namespace SmartShopAPI.Tests.Helpers
{
    public class UnitTestBase
    {
        protected readonly SmartShopDbContext _context;
        protected readonly ProductService _service;
        protected readonly IMapper _mapper;
        protected readonly SmartShopSeeder _seeder;

        public UnitTestBase()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SmartShopMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
            var options = new DbContextOptionsBuilder<SmartShopDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new SmartShopDbContext(options);
            _seeder = new SmartShopSeeder(_context, new PasswordHasher<User>());
            _seeder.Seed();
            _service = new ProductService(_context, _mapper);
        }
    }
}
