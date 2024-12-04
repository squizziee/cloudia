namespace Cloudia.API.Entities
{
    public class User
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public int user_profile_id { get; set; }
        public string? email { get; set; }
        public string? password_hash { get; set; }
    }
}
