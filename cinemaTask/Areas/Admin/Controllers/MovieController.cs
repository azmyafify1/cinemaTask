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
        private readonly Repository<cinemaTask.Models.Cinema> _cinemaRepository = new();
        private readonly Repository<Movie> _movieRepository = new();
        private readonly MovieSubImageRepository _movieSubImageRepository = new(new applicationDbContext());

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
        // ✅ Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetOne(m => m.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (movie is null)
                return RedirectToAction("NotFoundPage", "Home");

            ViewBag.Categories = new SelectList(await _categoryRepository.GetAsync(cancellationToken: cancellationToken), "Id", "Name", movie.CategoryId);
            ViewBag.Cinemas = new SelectList(await _cinemaRepository.GetAsync(cancellationToken: cancellationToken), "Id", "Name", movie.CinemaId);

            var subimgs = await _movieSubImageRepository.GetAsync(s => s.MovieId == id, tracked: false, cancellationToken: cancellationToken);
            ViewBag.Subimgs = subimgs.ToList();

            return View(movie);
        }

        // ✅ Edit (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie, IFormFile? MainImg, List<IFormFile>? Subimgs, CancellationToken cancellationToken)
        {
            var movieInDb = await _movieRepository.GetOne(m => m.Id == movie.Id, tracked: true, cancellationToken: cancellationToken);
            if (movieInDb is null)
                return RedirectToAction("NotFoundPage", "Home");

            // تحديث البيانات الأساسية
            movieInDb.Name = movie.Name;
            movieInDb.Description = movie.Description;
            movieInDb.Price = movie.Price;
            movieInDb.Status = movie.Status;
            movieInDb.DateTime = movie.DateTime;
            movieInDb.CategoryId = movie.CategoryId;
            movieInDb.CinemaId = movie.CinemaId;

            // تحديث الصورة الرئيسية إن وجدت
            if (MainImg is not null && MainImg.Length > 0)
            {
                var filename = Guid.NewGuid() + Path.GetExtension(MainImg.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Movies", filename);
                using var stream = System.IO.File.Create(path);
                await MainImg.CopyToAsync(stream, cancellationToken);
                movieInDb.MainImg = filename;
            }

            // تحديث الصور الفرعية
            if (Subimgs is not null && Subimgs.Count > 0)
            {
                var oldImgs = await _movieSubImageRepository.GetAsync(s => s.MovieId == movie.Id, tracked: true, cancellationToken: cancellationToken);
                foreach (var item in oldImgs)
                {
                    _movieSubImageRepository.Delete(item);
                }
                await _movieSubImageRepository.CommitAsync(cancellationToken);

                foreach (var subimg in Subimgs)
                {
                    var filename = Guid.NewGuid() + Path.GetExtension(subimg.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Movies", filename);
                    using var stream = System.IO.File.Create(path);
                    await subimg.CopyToAsync(stream, cancellationToken);

                    await _movieSubImageRepository.CreateAsync(new MovieSubimg
                    {
                        Image = filename,
                        MovieId = movie.Id
                    }, cancellationToken);
                }

                await _movieSubImageRepository.CommitAsync(cancellationToken);
            }

            await _movieRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        // ✅ Delete
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetOne(m => m.Id == id, tracked: true, cancellationToken: cancellationToken);
            if (movie is null)
                return RedirectToAction(nameof(Index));

            var subs = await _movieSubImageRepository.GetAsync(s => s.MovieId == id, tracked: true, cancellationToken: cancellationToken);
            foreach (var item in subs)
            {
                _movieSubImageRepository.Delete(item);
            }
            await _movieSubImageRepository.CommitAsync(cancellationToken);

            _movieRepository.Delete(movie);
            await _movieRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}