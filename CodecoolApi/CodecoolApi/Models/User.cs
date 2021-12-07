using System.ComponentModel.DataAnnotations.Schema;

namespace CodecoolApi.Models
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        [NotMapped]
        public string? Token { get; set; }
    }
}
