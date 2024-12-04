namespace Cloudia.API.Entities
{
    public class Comment
    {
        public int id { get; set; }
        public int user_profile_id { get; set; }
        public int post_id { get; set; }
        public string? text_content { get; set; }
        public DateTime posted_at { get; set; }
    }
}
