using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TutorApp2.Models
{
    [DynamoDBTable("message_all")]
    public class MessageDynamo
    {
        [DynamoDBHashKey]
        public string Messageid { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string RecieverName { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
