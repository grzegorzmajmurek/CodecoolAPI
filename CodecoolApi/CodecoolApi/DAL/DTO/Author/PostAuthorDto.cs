using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.DAL.DTO.Author
{
    public class PostAuthorDto
    {
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
