namespace CodecoolApi.DAL.DTO.Material
{
    public class PostMaterialDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public List<string>? Reviews { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
    }
}
