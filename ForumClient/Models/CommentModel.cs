using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public partial class CommentModel
    {
        [Key]
        public int comment_id { get; set; }
        public int topic_id { get; set; }
        public string username { get; set; }
        public string comment { get; set; }
        public int countReply { get; set; }
        public string create_at { get; set; }
    }
}
