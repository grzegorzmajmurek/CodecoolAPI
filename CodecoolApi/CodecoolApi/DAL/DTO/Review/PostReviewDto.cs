using System.ComponentModel.DataAnnotations;

namespace CodecoolApi.DAL.DTO.Review
{
    public class PostReviewDto
    {
        [MaxLength(100)]
        public string Text { get; set; }
        [Range(1, 10, ErrorMessage = "Score must be between 1 and 10")]
        public int ReviewScore { get; set; }
    }
}
