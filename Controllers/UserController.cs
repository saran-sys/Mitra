using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Mitra.Data;
using Mitra.Models;

namespace Mitra.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUserAction([FromForm] User user)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Name: {user.Name}, Email: {user.Email}, Password: {user.PasswordHash}");
                // Check if the email is already registered
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    TempData["Error"] = "User already exists!";
                    return RedirectToAction("RegisterUser");
                }

                // Save the user data with plain-text password (not recommended)
                _context.Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Name = user.Name,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash, // Store plain text
                    Role = 1 // Default role for new users
                });

                _context.SaveChanges();
                TempData["Success"] = "Registration successful!";
                return RedirectToAction("Index", "Home");
            }

            return View("RegisterUser", user);
        }




        // Profile Page
        public IActionResult Profile()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("UserLogin");
            }

            var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var currentUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            return View(currentUser);
        }

        // Login Page
        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLoginAction([FromForm] User user)
        {
            var foundUser = _context.Users
                .FirstOrDefault(u => u.Email.ToLower() == user.Email.ToLower()
                                  && u.PasswordHash == user.PasswordHash);

            if (foundUser == null)
            {
                TempData["Error"] = "Invalid credentials!";
                return RedirectToAction("UserLogin");
            }

            // Set session variables for authentication

            HttpContext.Session.SetString("UserId", foundUser.Id.ToString());
            HttpContext.Session.SetInt32("UserRole", foundUser.Role);

            TempData["Success"] = "Login successful!";

            Console.WriteLine($"Redirecting user. Role = {foundUser.Role}");
            // Redirect based on the role
            if (foundUser.Role == 0) // Admin role
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            
           return RedirectToAction("Index", "Home");
           
        }

        // Logout Action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Access Denied Page
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Helper Methods
        private bool IsAuthenticated()
        {
            return HttpContext.Session.GetString("UserId") != null;
        }

        private bool IsAdmin()
        {
            return IsAuthenticated() && HttpContext.Session.GetInt32("UserRole") == 0;
        }

        private bool IsUser()
        {
            return IsAuthenticated() && HttpContext.Session.GetInt32("UserRole") == 1;
        }
    }
}
