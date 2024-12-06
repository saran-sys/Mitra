using System;
namespace Mitra.Models
{
    public class Payment
    {
        public int PaymentId { get; set; } // Primary key
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}