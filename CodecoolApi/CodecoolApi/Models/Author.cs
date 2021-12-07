using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.Models
{
    public class Author : BaseEntity
    {
        [MaxLength(30)]
        public string UserName { get; set; }
        [MaxLength(30)]
        public string Description { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public int NumbersOfMaterials { get; set; }
    }
}
