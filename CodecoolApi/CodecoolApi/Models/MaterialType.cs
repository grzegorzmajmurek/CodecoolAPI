using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.Models
{
    public class MaterialType : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(250)]
        public string Definition { get; set; }
        public ICollection<Material>? Materials { get; set;}
    }
}
