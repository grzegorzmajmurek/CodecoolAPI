using CodecoolApi.Data;
using CodecoolApi.Models;
using CodecoolApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CodecoolApi.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateAsync(T entity)
        {
            _db.Set<T>().Attach(entity);
            var saveChanges = await _db.SaveChangesAsync();
            return saveChanges >= 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _db.Set<T>().Remove(entity);
            var saveChanges = await _db.SaveChangesAsync();
            return saveChanges >= 0;
        }

        public async Task<IAsyncEnumerable<T>> GetAllAsync()
        {
            return _db.Set<T>().AsAsyncEnumerable();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _db.Entry<T>(entity).State = EntityState.Modified;
            var saveChanges = await _db.SaveChangesAsync();
            return saveChanges >= 0;
        }

        public async Task<T> GetEntityByQueryEager(Func<DbSet<T>, IEnumerable<T>> includeQuery, Func<IEnumerable<T>, T> singleQuery)
        {
            IEnumerable<T> loadedEntities = includeQuery(_db.Set<T>());
            return singleQuery(loadedEntities);
        }

        public async Task<T> GetEntityByQuery(Func<T, bool> query)
        {
            return await _db.Set<T>().ToAsyncEnumerable().SingleOrDefaultAsync(query);
        }

        public async Task<IAsyncEnumerable<T>> EnlistAllEager(Func<DbSet<T>, IEnumerable<T>> includeQuery)
        {
            return includeQuery(_db.Set<T>()).ToAsyncEnumerable();
        }
    }
}
