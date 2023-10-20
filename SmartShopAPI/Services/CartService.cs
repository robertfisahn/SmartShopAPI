using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Data;
using SmartShopAPI.Entities;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos.CartItem;

namespace SmartShopAPI.Services
{
    public class CartService : ICartService
    {
        private readonly SmartShopDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;

        public CartService(SmartShopDbContext context, IUserContextService userContextService, IMapper mapper)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
        }

        public IEnumerable<CartItemDto> GetById(int userId)
        {
            var cartItems = _context.CartItems
                .Include(p => p.Product)
                .Where(x => x.UserId == userId)
                .ToList();
            var dto = _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
            return dto;
        }
        public int Create(int productId, CreateCartDto dto)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == productId) ?? throw new NotFoundException("Product not found");
            var userId = _userContextService.GetUserId();
            var cartItem = _mapper.Map<CartItem>(dto);
            cartItem.UserId = userId;
            cartItem.ProductId = productId;
            _context.Add(cartItem);
            _context.SaveChanges();
            return cartItem.Id;
        }

        public void Delete(int cartItemId)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(x => x.Id == cartItemId) ?? throw new NotFoundException("Cart item not found");
            _context.Remove(cartItem);
            _context.SaveChanges();
        }

        public void Update(int cartItemId, CreateCartDto dto)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(x => x.Id == cartItemId) ?? throw new NotFoundException("Cart item not found");
            _mapper.Map(dto, cartItem);
            _context.SaveChanges();
        }
    }
}
