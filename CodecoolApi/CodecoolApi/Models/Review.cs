namespace CodecoolApi.Models
{
    public class Review : BaseEntity
    {
        public Material Material { get; set; }
        public string? Text { get; set; }
        public int ReviewScore { get; set; }
    }
}
