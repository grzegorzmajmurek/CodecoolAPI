using CodecoolApi.Models;
using CodecoolApi.Repository;
using CodecoolApi.Repository.IRepository;

namespace CodecoolApi
{
    public static class Utilities
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Author>, Repository<Author>>();
            services.AddScoped<IRepository<Material>, Repository<Material>>();
            services.AddScoped<IRepository<MaterialType>, Repository<MaterialType>>();
            services.AddScoped<IRepository<Review>, Repository<Review>>();
        }
    }
}
