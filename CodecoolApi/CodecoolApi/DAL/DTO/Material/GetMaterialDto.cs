using CodecoolApi.Models;

namespace CodecoolApi.DAL.DTO.Material
{
    public class GetMaterialDto : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public List<string>? Reviews { get; set; }
        public DateTime PublisDate { get; set; }
        public string Author { get; set; }
    }
}
