using System;
using System.Collections.Generic;
using System.Text;

namespace TutorApp2.Models
{
    class Post
    {
        public string UID { get; set; }
        public string Title { get; set; }
        public string PosterEmail { get; set; }
        public string PosterName { get; set; }
        public string Grade { get; set; }
        public string Content { get; set; }
        public string Likes { get; set; }
        public List<string> Comments { get; set; }
        public DateTime PostTime { get; set; }
    }
}
