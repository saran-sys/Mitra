using System.ComponentModel.DataAnnotations;
namespace Mitra.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public Guid Id { get; set; }
        public User Users { get; set; }
        public DateTime Order_Date { get; set; }
        public int TotalAmount { get; set; }
        public string Status { get; set; }
        public string Delivery_Address { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}

