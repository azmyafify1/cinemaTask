using cinemaTask.DataAccess;
using cinemaTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace cinemaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public applicationDbContext _context = new();
        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().AsQueryable();

            return View(categories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            Category category = new();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(Category category ,IFormFile ImagePath)
        {
            if (ImagePath is not null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(ImagePath.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//images//Categories", filename);
                using var stream = System.IO.File.Create(path);
                ImagePath.CopyTo(stream);
                category.ImagePath = filename;
                _context.Categories.Add(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is null)
                return RedirectToAction( "Home");

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is null)
                return RedirectToAction( "Home");

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
