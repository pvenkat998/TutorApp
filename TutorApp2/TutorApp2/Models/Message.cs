﻿using System;
using System.Collections.Generic;
using System.Text;
using MvvmHelpers;
using Humanizer;
namespace TutorApp2.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string text { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsIncoming { get; set; }
        public bool IsOutgoing { get; set; }
    }
}