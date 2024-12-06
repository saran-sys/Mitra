using System;
namespace Mitra.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}