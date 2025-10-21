using cinemaTask.DataAccess;
using cinemaTask.Models;
using cinemaTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace cinemaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        public applicationDbContext _context = new();
        public IActionResult Index()
        {
            var cinemas = _context.Cinemas.AsNoTracking().ToList();
            return View(cinemas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //Cinema cinema = new();
            //ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            //ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile ImgPath)
        {
            if (ImgPath is not null && ImgPath.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgPath.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//images", fileName);

                //if(!System.IO.File.Exists(filePath))
                //{
                //    System.IO.File.Create(filePath);
                //}

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgPath.CopyTo(stream);
                }

                cinema.ImgPath = fileName;
            }

            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.Find(id);

            if (cinema is null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile ImgPath)
        {
            var cinemaInDB = _context.Cinemas.AsNoTracking().FirstOrDefault(e => e.Id == cinema.Id);

            if (cinemaInDB is null)
                return RedirectToAction("NotFoundPage", "Home");

            if (ImgPath is not null)
            {
                if (ImgPath.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgPath.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//images", fileName);

                    //if(!System.IO.File.Exists(filePath))
                    //{
                    //    System.IO.File.Create(filePath);
                    //}

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImgPath.CopyTo(stream);
                    }

                    cinema.ImgPath = fileName;
                }
            }
            else
            {
                cinema.ImgPath = cinemaInDB.ImgPath;
            }

            _context.Cinemas.Update(cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
            
        
        public IActionResult Delete(int id)
        {

            var cinema = _context.Cinemas.Find(id);
            if (cinema is null)
                return RedirectToAction( "Home");
            _context.Cinemas.Remove(cinema);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
