using cinemaTask.DataAccess;
using cinemaTask.Models;
//using cinemaTask.Models;
using cinemaTask.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace cinemaTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        //public applicationDbContext _context = new();
        private Repository<Cinema> _cinemaRepository = new();
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var cinemas = await _cinemaRepository.GetAsync( tracked: false , cancellationToken: cancellationToken) ;
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
        public async Task<IActionResult> Create(Cinema cinema, IFormFile ImgPath, CancellationToken cancellationToken)
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

            await _cinemaRepository.CreateAsync(cinema);
            await _cinemaRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var cinema = await _cinemaRepository.GetOne(e => e.Id == id, cancellationToken: cancellationToken);

            if (cinema is null)
                return RedirectToAction("NotFoundPage", "Home");

            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cinema cinema, IFormFile ImgPath, CancellationToken cancellationToken)
        {
            var cinemaInDB =await _cinemaRepository.GetOne(e => e.Id == cinema.Id, cancellationToken: cancellationToken);

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

            cinemaInDB.Name = cinema.Name;

            cinemaInDB.ImgPath = cinema.ImgPath;

            await _cinemaRepository.CommitAsync(cancellationToken);


            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken)
        {

            var cinema = await _cinemaRepository.GetOne(e => e.Id == id, cancellationToken: cancellationToken);

            if (cinema is null)
                return RedirectToAction("Home");
            _cinemaRepository.Delete(cinema);
            await _cinemaRepository.CommitAsync(cancellationToken);
            return RedirectToAction("Index");
        }
    }
}
