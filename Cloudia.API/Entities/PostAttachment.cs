namespace Cloudia.API.Entities
{
    public class PostAttachment
    {
        public int id { get; set; }
        public int post_id { get; set; }
        public string? source_url { get; set; }
    }
}
