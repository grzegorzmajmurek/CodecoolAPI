namespace CodecoolApi.Models
{
    public class MaterialType : BaseEntity
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public ICollection<Material>? Materials { get; set;}
    }
}
