namespace CodecoolApi.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<IAsyncEnumerable<T>> GetAllAsync();
        Task<bool> DeleteAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> CreateAsync(T entity);
    }
}
