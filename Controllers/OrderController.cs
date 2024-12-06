using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mitra.Data;
using Mitra.Models;

namespace Mitra.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // POST: Order/PlaceOrder
        [HttpPost]
        public IActionResult PlaceOrder(Guid userId, string deliveryAddress)
        {
            // Fetch cart items for the user
            var cartItems = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.Id == userId)
                .ToList();

            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty.");
            }

            // Calculate total amount
            var totalAmount = cartItems.Sum(c => c.Quantity * c.Product.Price);

            // Create a new order
            var newOrder = new Order
            {
                OrderId = Guid.NewGuid(),
                Id = userId,
                Order_Date = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Pending",
                Delivery_Address = deliveryAddress
            };

            _context.Orders.Add(newOrder);

            // Add order details
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    OrderDetailId = Guid.NewGuid(),
                    OrderId = newOrder.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price,
                    SubTotal = item.Quantity * item.Product.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }

            // Clear cart after placing the order
            _context.Carts.RemoveRange(cartItems);

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("OrderConfirmation", new { orderId = newOrder.OrderId });
        }

        // GET: Order/OrderConfirmation
        public IActionResult OrderConfirmation(Guid orderId)
        {
            var order = _context.Orders
                .Include(o => o.Users)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return View(order);
        }

        // GET: Order/Details
        public IActionResult Details(Guid orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return View(order);
        }

        // GET: Order/UserOrders
        public IActionResult UserOrders(Guid userId)
        {
            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.Id == userId)
                .ToList();

            return View(orders);
        }

        // POST: Order/UpdateStatus (Admin Only)
        [HttpPost]
        public IActionResult UpdateStatus(Guid orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.Status = status;
            _context.SaveChanges();

            return RedirectToAction("Details", new { orderId = orderId });
        }
    }
}
