/*using System;
using Microsoft.AspNetCore.Mvc;
using Mitra.Data;
using Mitra.Models;

namespace Mitra.Controllers
{
    public class CategoryController : Controller
    private readonly AppDbContext dbContext;
    public CategoryController(AppDbContext dbContext)
    {
        {
            {
                this.dbContext = dbContext;
            }
            [HttpGet]
            public IActionResult Add()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult>
            Add(AddCategoryViewModel viewModel)
            {
                var Category = new Categories
                {
                    CategoryName = viewModel.CategoryName,
                    Description = viewModel.Description,
                    DateAddded = viewModel.DateAdded,
                };
                await dbContext.Categories.AddAsync(Category);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Categories", "Category");
            }
        }
    }
}*/

using Microsoft.AspNetCore.Mvc;
using Mitra.Data;
using Mitra.Models;
using System;
using System.Linq;

namespace Mitra.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Category/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: Category/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                category.DateAdded = DateTime.Now;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/{id}
        public IActionResult Edit(Guid id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Category category)
        {
            if (id == category.CategoryId)
            {
                if (ModelState.IsValid)
                {
                    var existingCategory = _context.Categories.Find(id);
                    if (existingCategory == null)
                    {
                        return NotFound();
                    }

                    existingCategory.CategoryName = category.CategoryName;
                    existingCategory.Description = category.Description;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(category);
            }

            return BadRequest();
        }

        // GET: Category/Delete/{id}
        public IActionResult Delete(Guid id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
