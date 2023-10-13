﻿using System.Text.Json.Serialization;

namespace SmartShopAPI.Models.Dtos
{
    public class CreateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}