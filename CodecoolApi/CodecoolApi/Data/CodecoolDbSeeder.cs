//using CodecoolApi.Models;
//using Microsoft.EntityFrameworkCore;

//namespace CodecoolApi.Data
//{
//    public class CodecoolDbSeeder
//    {
//        private readonly ApplicationDbContext _context;

//        public CodecoolDbSeeder(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public void Seed()
//        {
//            if (!_context.MaterialsTypes.Any())
//            {
//                AddMaterialTypes(_context);
//                AddReviews(_context);
//                AddAuthors(_context);
//                AddMaterials(_context);
//            }
//        }

//        private void AddMaterialTypes(ApplicationDbContext context)
//        {
//            var materialTypes = new List<MaterialType>()
//                {
//                    new MaterialType() { Name = "Test", Definition = "TEst"}
//                };
//            context.MaterialsTypes.AddRange(materialTypes);
//            context.SaveChanges();
//        }

//        private void AddReviews(ApplicationDbContext context)
//        {
//            var reviews = new List<Review>()
//                {
//                    new Review() { Text = "Test", ReviewScore = 5 }
//                };
//            context.Reviews.AddRange(reviews);
//            context.SaveChanges();
//        }

//        private void AddAuthors(ApplicationDbContext context)
//        {
//            var authors = new List<Author>()
//            {
//                new Author() { Name = "Test", Description = "Test", NumbersOfMaterials = 0}
//            };
//            context.Authors.AddRange(authors);
//            context.SaveChanges();
//        }

//        private void AddMaterials(ApplicationDbContext context)
//        {
//            var materials = new List<Material>()
//                {
//                    new Material() {
//                        Title = "Test",
//                        Description = "Test",
//                        Location = "Test",
//                        Type = context.MaterialsTypes.Single(x => x.Name == "Test"),
//                        PublishDate = DateTime.UtcNow,
//                        Author = context.Authors.Single(x => x.Name == "Test"),
//                        Reviews = new List<Review>()
//                    }
//                };
//            context.Materials.AddRange(materials);
//            context.SaveChanges();
//        }
//    }
//}

