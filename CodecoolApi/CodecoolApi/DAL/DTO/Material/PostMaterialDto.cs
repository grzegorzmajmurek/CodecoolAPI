using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.DAL.DTO.Material
{
    public class PostMaterialDto
    {
        [MaxLength(30)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        [MaxLength(25)]
        public string Location { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
