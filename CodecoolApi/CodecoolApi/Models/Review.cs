namespace CodecoolApi.Models
{
    public class Review : BaseEntity
    {
        public string Text { get; set; }
        public int ReviewScore { get; set; }
        public Material? Material { get; set; }
    }
}
