using System;
using System.Collections.Generic;

#nullable disable

namespace ForumAPI.Models
{
    public partial class Category
    {
        public Category()
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
