using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.Models
{
    public class Material : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(350)]
        public string Description { get; set; }
        [MaxLength(50)]
        public string Location { get; set; }
        public MaterialType? Type { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public DateTime PublishDate { get; set; }
        public Author? Author { get; set; }
    }
}
