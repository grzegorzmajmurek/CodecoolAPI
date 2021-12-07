using CodecoolApi.Models;

namespace CodecoolApi.DAL.DTO.Author
{
    public class GetAuthorDto : BaseEntity
    {
        public string UserName { get; set; }
        public string Description { get; set; }
        public List<string> Materials { get; set; }
        public int NumbersOfMaterials { get; set; }
    }
}
