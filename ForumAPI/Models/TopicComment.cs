using System;
using System.Collections.Generic;

#nullable disable

namespace ForumAPI.Models
{
    public partial class TopicComment
    {
        public TopicComment()
        {
            TopicReplies = new HashSet<TopicReply>();
        }

        public int CommentId { get; set; }
        public int? TopicId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string CreatedAt { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual ICollection<TopicReply> TopicReplies { get; set; }
    }
}
