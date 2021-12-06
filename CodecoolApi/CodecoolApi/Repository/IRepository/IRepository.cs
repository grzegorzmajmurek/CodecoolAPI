using CodecoolApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CodecoolApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        public Task<T> GetAsync(int id);
        public Task<IAsyncEnumerable<T>> GetAllAsync();
        public Task<T> GetEntityByQueryEager(Func<DbSet<T>, IEnumerable<T>> query, Func<IEnumerable<T>, T> singleQuery);
        public Task<bool> DeleteAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> CreateAsync(T entity);
    }
}
