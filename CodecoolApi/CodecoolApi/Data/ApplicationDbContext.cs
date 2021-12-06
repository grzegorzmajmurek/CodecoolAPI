using CodecoolApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CodecoolApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialType> MaterialsTypes { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
