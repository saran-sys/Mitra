using System;
namespace Mitra.Models
{
    public class Feedback
    {
        public Guid FeedbackId { get; set; }
        public Guid UserId { get; set; }
        public User Users { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime DateSubmitted { get; set; }
    }
}