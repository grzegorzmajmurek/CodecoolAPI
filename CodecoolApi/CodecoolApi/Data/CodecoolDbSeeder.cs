using CodecoolApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CodecoolApi.Data
{
    public static class CodecoolDbSeeder
    {
        public static void Initialize(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().HasData(
                new Material
                {
                    Id = 1,
                    Title = "Nowa publikacja",
                    Description = "Najlepsza publikacja jaka jest",
                    Location = "Zachodnia 15/69",
                    Type = new MaterialType
                    {
                        Id = 1,
                        Name = "Material type 1",
                        Definition = "Definicja 1"
                    },
                    Reviews = new List<Review>(),
                    PublishDate = DateTime.Now,
                    AuthorId = 1,
                    Author = new Author
                    {
                        Id = 1,
                        Name = "Michał",
                        Description = "Michała publikacja",
                        Materials = new List<Material>(),
                        NumbersOfMaterials = 1
                    }
                }
                );
        }
    }
}
