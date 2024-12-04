namespace Cloudia.API.Entities
{
    public class Like
    {
        public int id { get; set; }
        public int user_profile_id { get; set; }
        public int post_id { get; set; }
        public DateTime posted_at { get; set; }
    }
}
