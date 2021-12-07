using CodecoolApi.Models;

namespace CodecoolApi.DAL.DTO.Review
{
    public class GetReviewDto : BaseEntity
    {
        public string Text { get; set; }
        public int ReviewScore { get; set; }
        public string Material { get; set; }
    }
}
