using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumClient.Models
{
    public class Confirmed_docter_view
    {
        public int id { get; set; }
        //[StringLength(15, ErrorMessage = "Name length can't be more than 15.")]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Experience { get; set; }
        public string Qualification { get; set; }
        public string Professional { get; set; }
        public string Mess { get; set; }
    }
}
