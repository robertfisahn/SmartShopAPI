﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Data;
using SmartShopAPI.Entities;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Interfaces;

namespace SmartShopAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly SmartShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public OrderService(SmartShopDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public Order GetById(int orderId)
        {
            var order = _context.Orders
                .Include(x => x.OrderItems)
                .SingleOrDefault(o => o.Id == orderId) ?? throw new NotFoundException("Order not found");
            return order;
        }
        public int Create()
        {
            var userId = _userContextService.GetUserId();
            var cartItems = GetCartItems(userId);
            Order order = CreateOrder(cartItems, userId);
            var orderItems = CreateOrderItems(cartItems, order.Id);
            UpdateStockQuantity(orderItems);
            ClearCart(cartItems);
            _context.SaveChanges();
            return order.Id;
        }
        public void ClearCart(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
        }
        public Order CreateOrder(List<CartItem> cartItems, int userId)
        {
            var addressId = GetUserAddressId(userId);
            Order order = new()
            {
                TotalPrice = cartItems.Sum(x => x.Quantity * x.Product.Price),
                UserId = userId,
                AddressId = addressId
            };
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }
        public List<CartItem> GetCartItems(int userId)
        {
            var cartItems = _context.CartItems
                .Include(x => x.Product)
                .Where(p => p.UserId == userId)
                .ToList();
            if (!cartItems.Any())
            {
                throw new NotFoundException("Cart items not found");
            }
            return cartItems;
        }

        public int GetUserAddressId(int userId)
        {
            var userAddress = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.AddressId)
                .SingleOrDefault() ?? throw new NotFoundException("User not found");
            return userAddress;
        }
        public List<OrderItem> CreateOrderItems(List<CartItem> cartItems, int orderId)
        {
            var orderItems = _mapper.Map<List<OrderItem>>(cartItems);
            foreach (var item in orderItems)
            {
                item.OrderId = orderId;
            }
            _context.OrderItems.AddRange(orderItems);
            return orderItems;
        }

        public void UpdateStockQuantity(List<OrderItem> orderItems)
        {
            foreach (var item in orderItems)
            {
                var product = _context.Products.SingleOrDefault(x => x.Id == item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                }
            }
        }
    }


}
