﻿using System;
using Mitra.Models;

namespace Mitra.Models
{
   public class Cart
    {
        public Guid CartId { get; set; }
        public Guid Id { get; set; }
        public User? User { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

        public int TotalPrice()
        {
            if (Product == null)
            {
                return 0;
            }
            return Product.Price * Quantity;
        }
    }
}