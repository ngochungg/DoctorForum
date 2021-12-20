using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public partial class TopicModel
    {
        [Key]
        public int Topic_Id { set; get; }
        public int Categogies_id { set; get; }
        public string Username { set; get; }
        public string Title { set; get; }
        public string Contents { set; get; }
        public string Created_at { set; get; }

    }
}
