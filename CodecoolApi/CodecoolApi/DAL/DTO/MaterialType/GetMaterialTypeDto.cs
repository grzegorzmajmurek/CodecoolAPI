using CodecoolApi.Models;

namespace CodecoolApi.DAL.DTO.MaterialType
{
    public class GetMaterialTypeDto : BaseEntity
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public List<string> Materials { get; set; }
    }
}
