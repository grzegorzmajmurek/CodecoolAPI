namespace CodecoolApi.Models
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public int NumbersOfMaterials { get; set; }
    }
}
