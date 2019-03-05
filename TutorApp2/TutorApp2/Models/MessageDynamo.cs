using System;
using System.Collections.Generic;
using System.Text;

namespace TutorApp2.Models
{
    public class MessageDynamo
    {
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
