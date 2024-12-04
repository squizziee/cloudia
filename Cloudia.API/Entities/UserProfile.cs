namespace Cloudia.API.Entities
{
    public class UserProfile
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int role_id { get; set; }
        public int ban_status_id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? avatar_url { get; set; }
        public string? location { get; set; }
        public string? biography { get; set; }
        public int? age { get; set; }
        public DateTime created_at{ get; set; }
    }
}
