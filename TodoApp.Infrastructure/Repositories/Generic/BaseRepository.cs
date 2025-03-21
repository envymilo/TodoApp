using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Entities;

namespace TodoApp.Infrastructure.Repositories.Generic
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly TodoAppDbContext _context;
        protected readonly DbSet<T> dbSet;


        public BaseRepository(TodoAppDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
        {
            return await dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            var added = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, T entity)
        {

            var existingEntity = await dbSet.FindAsync(id);

            if (existingEntity == null)
            {
                return false;
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRange(IEnumerable<T> entities)
        {
            var keys = entities.Select(e => GetEntityKey(e)).ToList();

            // Fetch all existing records in one query
            var existingEntities = await _context.Set<T>().Where(e => keys.Contains(GetEntityKey(e))).ToListAsync();

            var entityDictionary = existingEntities.ToDictionary(e => GetEntityKey(e));

            foreach (var entity in entities)
            {
                if (!entityDictionary.TryGetValue(GetEntityKey(entity), out var existingEntity))
                {
                    return false;
                    throw new InvalidOperationException("Entity not found.");
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private Guid GetEntityKey(T entity)
        {
            var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).First();
            return (Guid)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
                throw new InvalidOperationException($"Entity with ID {id} not found.");
            }

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entitiesToDelete = await _context.Set<T>().Where(e => ids.Contains(GetEntityKey(e))).ToListAsync();

            if (!entitiesToDelete.Any())
            {
                return false;
                throw new InvalidOperationException("No entities found for deletion.");
            }

            _context.Set<T>().RemoveRange(entitiesToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
