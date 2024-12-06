using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mitra.Data; // Your DbContext
using Mitra.Migrations;
using Mitra.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mitra.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // View Cart
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId(); // Replace with your method to get the logged-in user ID
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.Id == userId) // Match with `Id` property in `Cart` model
                .ToListAsync();

            var totalPrice = cartItems.Sum(c => c.TotalPrice());

            ViewBag.TotalPrice = totalPrice; // Pass total price to the view
            return View(cartItems);
        }

        // Add to Cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity = 1)
        {
            var userId = GetUserId(); // Your logic for getting the logged-in user ID

            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                _context.Carts.Update(existingCartItem);
            }
            else
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }

                var cartItem = new Cart
                {
                    CartId = Guid.NewGuid(),
                    Id = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Product = product
                };
                _context.Carts.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Cart");
        }


        // Update Quantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid cartId, int quantity)
        {
            var cartItem = await _context.Carts
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                _context.Carts.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
                _context.Carts.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Remove from Cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid cartId)
        {
            var cartItem = await _context.Carts.FindAsync(cartId);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Clear Cart
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            var cartItems = await _context.Carts
                .Where(c => c.Id == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId(); // Replace with your method to get the logged-in user ID

            // Retrieve all cart items for the user
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.Id == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                ModelState.AddModelError("", "Your cart is empty!");
                return RedirectToAction(nameof(Index));
            }

            // Create an Order
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                Id = userId,
                Order_Date = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(c => c.TotalPrice()),
                OrderDetails = cartItems.Select(c => new OrderDetails
                {
                    OrderDetailId = Guid.NewGuid(),
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            // Clear the cart
            _context.Carts.RemoveRange(cartItems);

            // Save changes
            await _context.SaveChangesAsync();

            // Redirect to an order confirmation page
            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        // Helper method to get user ID (stub)
        private Guid GetUserId()
        {
            // Replace with your logic to retrieve the logged-in user ID
            return Guid.NewGuid(); // Dummy value for now
        }
    }
}
