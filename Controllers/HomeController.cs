using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mitra.Data;
using Mitra.Models;

namespace Mitra.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

   
    // GET: Product
    public IActionResult Index()
    {
        //var products = _context.Products.ToList(); // Retrieve all products
        //return View(products); // Pass to the view
        var featuredProducts = _context.Products.Take(6).ToList(); // Example fetching products
        ViewData["FeaturedProducts"] = featuredProducts;
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }


    public IActionResult AdminDashboard()
    {

        ViewBag.TotalProducts = _context.Products.Count();
        ViewBag.TotalCategories = _context.Categories.Count();
        ViewBag.TotalUsers = _context.Users.Count();
        ViewBag.TotalOrders = _context.Orders.Count();
        // Example content for AdminDashboard
        ViewBag.Message = "Welcome to Admin Dashboard";
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

    
