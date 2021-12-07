using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.DAL.DTO.MaterialType
{
    public class PostMaterialTypeDto
    {
        [MaxLength(40)]
        public string Name { get; set; }
        [MaxLength(70)]
        public string Definition { get; set; }
    }
}
