using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public partial class TopicModel
    {
        [Key]
        public int topic_id { get; set; }
        public int cate_id { get; set; }
        public string usename { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }

    }
}
