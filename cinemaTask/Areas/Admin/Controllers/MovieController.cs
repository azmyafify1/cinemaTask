using cinemaTask.DataAccess;
using cinemaTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace cinemaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        public applicationDbContext _context = new();
        public IActionResult Index()
        {
            var Movies = _context.Movies.Include(m => m.Category).Include(m => m.Cinema).AsNoTracking();
            return View(Movies.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            Movie movie = new();
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            return View(movie);
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile MainImg,
            List<IFormFile> Subimgs)
        {
            if (MainImg is not null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//images//Movies", filename);
                using var stream = System.IO.File.Create(path);
                MainImg.CopyTo(stream);
                movie.MainImg = filename;

            }
            var movieCreated = _context.Movies.Add(movie);
            _context.SaveChanges();

            if (Subimgs is not null && Subimgs.Count > 0)
            {
                foreach (var item in Subimgs)
                {
                    var filename = Guid.NewGuid().ToString()
                        + Path.GetExtension(item.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot\\images\\Movies", filename);

                    using (var stream = System.IO.File.Create(path))
                    {
                        item.CopyTo(stream);
                    }
                    _context.movieSubimgs.Add(new()
                    {
                        Image = filename,
                        MovieId = movieCreated.Entity.Id
                    });
                }
                _context.SaveChanges();
            }


            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie is null)
                return RedirectToAction("Home");
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            ViewBag.Subimgs = _context.movieSubimgs.Where(m => m.MovieId == id).ToList();
            return View(movie);

        }
        [HttpPost]
        public IActionResult Edit(Movie movie, IFormFile MainImg, List<IFormFile> Subimgs)
        {

            if (MainImg is not null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//images//Movies", filename);
                using var stream = System.IO.File.Create(path);
                MainImg.CopyTo(stream);
                movie.MainImg = filename;
            }
            else
            {
                var oldmovie = _context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == movie.Id);
                movie.MainImg = oldmovie!.MainImg;

            }
            if (Subimgs != null)
            {
                var oldimg = _context.movieSubimgs.Where(m => m.MovieId == movie.Id);
                foreach (var item in oldimg)
                {
                    _context.movieSubimgs.Remove(item);
                }
                _context.SaveChanges();
                foreach (var item in Subimgs)
                {
                    var filename = Guid.NewGuid().ToString()
                        + Path.GetExtension(item.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot\\images\\Movies", filename);
                    using (var stream = System.IO.File.Create(path))
                    {
                        item.CopyTo(stream);
                    }
                    _context.movieSubimgs.Add(new()
                    {
                        Image = filename,
                        MovieId = movie.Id
                    });
                }


            }
            else
            {
                var oldSubimgs = _context.movieSubimgs.Where(m => m.MovieId == movie.Id).ToList();
                foreach (var item in oldSubimgs)
                {
                    _context.movieSubimgs.Add(new()
                    {
                        Image = item.Image,
                        MovieId = movie.Id
                    });
                }
            }
            var movieCreated = _context.Movies.Update(movie);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {

            var movie = _context.Movies.Find(id);
            if (movie is null)
                return RedirectToAction("Home");
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
