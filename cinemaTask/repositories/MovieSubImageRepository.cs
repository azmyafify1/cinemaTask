using cinemaTask.DataAccess;
using cinemaTask.Models;

namespace cinemaTask.repositories
{
    public class MovieSubImageRepository : Repository<MovieSubimg>
    {
        private readonly applicationDbContext _context;

        public MovieSubImageRepository(applicationDbContext context) : base()
        {
            _context = context;
        }

        public void RemoveRange(IEnumerable<MovieSubimg> entities)
        {
            _context.Set<MovieSubimg>().RemoveRange(entities);
        }

        public async Task RemoveByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
        {
            var items = _context.Set<MovieSubimg>().Where(x => x.MovieId == movieId);
            _context.RemoveRange(items);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public IQueryable<MovieSubimg> GetByMovieId(int movieId)
        {
            return _context.Set<MovieSubimg>().Where(x => x.MovieId == movieId);
        }
    }
}
