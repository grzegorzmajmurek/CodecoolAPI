using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.DAL.DTO.User
{
    public class AuthenticationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
