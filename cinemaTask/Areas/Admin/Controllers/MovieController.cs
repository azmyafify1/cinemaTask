using cinemaTask.DataAccess;
using cinemaTask.Models;
using cinemaTask.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace cinemaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly Repository<Category> _categoryRepository = new();
        private readonly Repository<Cinema> _cinemaRepository = new();
        private readonly Repository<Movie> _movieRepository = new();
        private readonly MovieSubImageRepository _movieSubImageRepository = new(new applicationDbContext() );

        public IActionResult Index()
        {
            var movies = _movieRepository
                .GetAllIncluding(m => m.Category, m => m.Cinema)
                .AsNoTracking();

            return View(movies.AsEnumerable());
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            Movie movie = new();
            ViewBag.Categories = new SelectList(await _categoryRepository.GetAsync(cancellationToken: cancellationToken), "Id", "Name");
            ViewBag.Cinemas = new SelectList(await _cinemaRepository.GetAsync(cancellationToken: cancellationToken), "Id", "Name");
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie, IFormFile MainImg, List<IFormFile> Subimgs, CancellationToken cancellationToken)
        {
            // حفظ الصورة الرئيسية
            if (MainImg is not null)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Movies", filename);
                using var stream = System.IO.File.Create(path);
                await MainImg.CopyToAsync(stream, cancellationToken);
                movie.MainImg = filename;
            }

            // حفظ الفيلم أولاً
            await _movieRepository.CreateAsync(movie, cancellationToken);
            await _movieRepository.CommitAsync(cancellationToken);

            // حفظ الصور الفرعية
            if (Subimgs is not null && Subimgs.Count > 0)
            {
                foreach (var item in Subimgs)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(item.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Movies", filename);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await item.CopyToAsync(stream, cancellationToken);
                    }

                    await _movieSubImageRepository.CreateAsync(
                        new MovieSubimg
                        {
                            Image = filename,
                            MovieId = movie.Id
                        },
                        cancellationToken
                    );
                }

                await _movieSubImageRepository.CommitAsync(cancellationToken);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
