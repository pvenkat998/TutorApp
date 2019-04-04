using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TutorApp2.Models
{
    [DynamoDBTable("forum_posts")]
    class Post
    {
        [DynamoDBHashKey]
        public string UID { get; set; }
        [DynamoDBRangeKey]
        public string Grade { get; set; }
        public string Title { get; set; }
        public string PosterEmail { get; set; }
        public string PosterName { get; set; }
        public string Content { get; set; }
        public string Likes { get; set; }
        public List<string> Comments { get; set; }
        public DateTime PostTime { get; set; }
    }
}
