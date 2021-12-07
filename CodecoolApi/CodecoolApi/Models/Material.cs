using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.Models
{
    public class Material : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public MaterialType? Type { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public DateTime PublishDate { get; set; }
        public Author? Author { get; set; }
    }
}
