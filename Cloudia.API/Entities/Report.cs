namespace Cloudia.API.Entities
{
    public class Report
    {
        public int id { get; set; }
        public int sender_id { get; set; }
        public int receiver_id { get; set; }
        public int violation_id { get; set; }
        public int post_id { get; set; }
    }
}
