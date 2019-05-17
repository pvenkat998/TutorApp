﻿using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TutorApp2.Models
{
    [DynamoDBTable("forum_posts")]
    public class Report
    {
        [DynamoDBHashKey]
        public string UID { get; set; }
        [DynamoDBRangeKey]
        public string Grade { get; set; }
        public string Title { get; set; }
        public string PosterEmail { get; set; }
        public string PosterName { get; set; }
        public string Content { get; set; }
        public string PostType { get; set; }
        public DateTime PostTime { get; set; }
        public string PosterPicPath { get; set; }
        public string StudentEmail { get; set; }
    }
    public class Comm
    {
        public string Commentor { get; set; }
        public string Comment { get; set; }
    }
    [DynamoDBTable("forum_posts")]
    public class Post
    {
        [DynamoDBHashKey]
        public string UID { get; set; }
        [DynamoDBRangeKey]
        public string Grade { get; set; }
        public string Title { get; set; }
        public string PosterEmail { get; set; }
        public string PosterName { get; set; }
        public string Content { get; set; }
        public string PostType { get; set; }
        public List<Comm> Comments { get; set; }
        public DateTime PostTime { get; set; }
        public string PosterPicPath { get; set; }
        public string PostPicPath { get; set; }
    }
}
