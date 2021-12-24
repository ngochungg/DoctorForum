using System.ComponentModel.DataAnnotations;

namespace ForumClient.Models
{
    public class ReplyModel
    {
        [Key]
        public int reply_id { get; set; }
        public int comment_id { get; set; }
        public string username { get; set; }
        public string Reply { get; set; }
        public string create_at { get; set; }
    }
}
