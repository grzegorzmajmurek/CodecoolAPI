using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.Models
{
    public class Review : BaseEntity
    {
        [MaxLength(250)]
        public string Text { get; set; }
        [Range(0, 10)]
        public int ReviewScore { get; set; }

        public Material? Material { get; set; }
    }
}
