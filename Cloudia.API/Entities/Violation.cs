namespace Cloudia.API.Entities
{
    public class Violation
    {
        public int id {  get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int ban_days { get; set; }
    }
}
