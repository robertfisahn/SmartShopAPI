﻿using SmartShopAPI.Models;
using System.Text.Json.Serialization;

namespace SmartShopAPI.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product {  get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
