using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public class Topic
    {
        public Topic()
        {
            TopicComments = new HashSet<TopicComment>();
        }

        public int TopicId { get; set; }
        public int? CategogiesId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string CreatedAt { get; set; }

        public virtual Categogy Categogies { get; set; }
        public virtual ICollection<TopicComment> TopicComments { get; set; }
    }
}
