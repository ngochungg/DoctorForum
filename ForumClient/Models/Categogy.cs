using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public class Categogy
    {
        public Categogy()
        {
            Topics = new HashSet<Topic>();
        }

        public int CateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
    }
}
