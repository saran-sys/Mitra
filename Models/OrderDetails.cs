using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mitra.Models;

namespace Mitra.Models
{
    public class OrderDetails
    {
        [Key]
        public Guid OrderDetailId { get; set; }

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public Guid ProductId { get; set; }

        // Ensure this navigation property is defined
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
        public int SubTotal { get; set; }
    }
}
