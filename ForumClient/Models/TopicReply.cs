using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public class TopicReply
    {
        public int ReplyId { get; set; }
        public int? CommentId { get; set; }
        public string Username { get; set; }
        public string Reply { get; set; }
        public string CreatedAt { get; set; }

        public virtual TopicComment Comment { get; set; }
    }
}
