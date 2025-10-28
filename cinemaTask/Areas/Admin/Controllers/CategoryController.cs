using cinemaTask.DataAccess;
using cinemaTask.Models;
using cinemaTask.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace cinemaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private Repository<Category> _categoryRepository = new();
        private Repository<Cinema> _CinemaRepository = new();


        // public applicationDbContext _context = new();
        public async Task<IActionResult> Index( CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAsync(tracked: false);

            return View(categories.AsEnumerable());
        }
        [HttpGet]
        public async Task<IActionResult> CreateAsync( Category category , CancellationToken cancellationToken)
        {
            Category categoryRepository = new();
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAsync(cancellationToken: cancellationToken), "Id", "Name");
            ViewBag.Cinemas = new SelectList(await _CinemaRepository.GetAsync(cancellationToken: cancellationToken), "Id", "Name");
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category ,IFormFile ImagePath, CancellationToken cancellationToken )
        {
            if (ImagePath is not null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(ImagePath.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//images//Categories", filename);
                using var stream = System.IO.File.Create(path);
                ImagePath.CopyTo(stream);
                category.ImagePath = filename;
                await _categoryRepository.CreateAsync(category  , cancellationToken );
                await _categoryRepository.CommitAsync(cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var category = (await _categoryRepository.GetAsync(e => e.Id == id , cancellationToken : cancellationToken)).FirstOrDefault();

            if (category is null)
                return RedirectToAction( "Home");

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category, CancellationToken cancellationToken)
        {
             _categoryRepository.Update(category);
            await _categoryRepository.CommitAsync( cancellationToken);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken )
        {
            var category =( await _categoryRepository.GetAsync(e => e.Id == id, cancellationToken: cancellationToken)).FirstOrDefault();


            if (category is null)
                return RedirectToAction( "Home");

           _categoryRepository.Delete(category);
           await _categoryRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
