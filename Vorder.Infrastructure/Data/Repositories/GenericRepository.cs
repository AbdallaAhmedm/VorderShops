using Microsoft.EntityFrameworkCore;
using Vorder.Application.Interfaces.Repositories;

namespace Vorder.Infrastructure.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        private DbSet<TEntity> _entities;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }


        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(entity);
        }
        public async Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var items = await _entities
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            return items;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
