using System;
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
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Reciever_Surname { get; set; }
        public string Rec_ImageSrc { get; set; }
        public string MsgPicSrc { get; set; }
        public bool IsText { get; set; }
        public bool IsPic { get; set; }
        public bool IsIncoming { get; set; }
        public bool IsOutgoing { get; set; }
    }
}