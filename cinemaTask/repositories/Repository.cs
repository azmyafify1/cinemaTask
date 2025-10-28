using cinemaTask.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace cinemaTask.repositories
{
    public class Repository<T> where T : class
    {
        private readonly applicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository()
        {
            _context = new applicationDbContext();
            _dbSet = _context.Set<T>();
        }

        // CREATE
        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entityCreated = await _dbSet.AddAsync(entity, cancellationToken);
            return entityCreated.Entity;
        }

        // UPDATE
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // DELETE
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // GET ALL + FILTER
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? expression = null,
            bool tracked = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> entities = _dbSet;

            if (expression is not null)
                entities = entities.Where(expression);

            if (!tracked)
                entities = entities.AsNoTracking();

            return await entities.ToListAsync(cancellationToken);
        }

        // GET ONE
        public async Task<T?> GetOne(Expression<Func<T, bool>>? expression = null,
            bool tracked = true,
            CancellationToken cancellationToken = default)
        {
            return (await GetAsync(expression, tracked, cancellationToken)).FirstOrDefault();
        }

        // INCLUDE RELATIONS
        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        // SAVE CHANGES
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
    }
}
