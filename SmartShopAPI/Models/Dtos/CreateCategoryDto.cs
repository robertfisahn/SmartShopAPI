﻿using System.ComponentModel.DataAnnotations;

namespace SmartShopAPI.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
