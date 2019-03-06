using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Models
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessagePageSimple : ContentPage
    {
        public MessagePageSimple ()
        {
            List<Message> messagelist =  MessageDB();
          
            BindingContext = messagelist;
            InitializeComponent ();
		}
        private async void SendMessage(object sender, EventArgs e)
        {

            MessageDynamo mes = new MessageDynamo
            {
                Messageid = Guid.NewGuid().ToString(),
                Sender = App.cur_user.email,
                Reciever = App.User_Recepient.Email,
                Message = Messagetosend.Text,
                TimeStamp = DateTime.Now
            };

            Console.WriteLine("aa");
            await SaveAsync(mes);

            msg.ItemsSource  = null;
            List<Message> messagelist = MessageDB();
            msg.ItemsSource = messagelist;
            Messagetosend.Text = "";
        }
        public List<Message> MessageDB()
        {
            QueryFilter filter = new QueryFilter();
            filter.AddCondition("Sender", QueryOperator.Equal, App.cur_user.email);
            filter.AddCondition("Reciever", QueryOperator.Equal, App.User_Recepient.Email);
            var client = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(client);
            var searchm = context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Sender-Reciever-index",
                Filter=filter
            });
            Console.WriteLine("bb items retrieved");

            //download images
            App.messearchResponse = searchm.GetRemainingAsync().Result;
            QueryFilter filter2 = new QueryFilter();
            filter2.AddCondition("Reciever", QueryOperator.Equal, App.cur_user.email);
            filter2.AddCondition("Sender", QueryOperator.Equal, App.User_Recepient.Email);
            var searchm2 = context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Reciever-Sender-index",
                Filter = filter2

            });
            Console.WriteLine(App.messearchResponse);
            App.messearchResponse2 = searchm2.GetRemainingAsync().Result;
            //working with list
            List<Message> messagelist = new List<Message>();
            messagelist.Add(new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = new DateTime(2008, 5, 1, 8, 30, 52), IsIncoming = true, IsOutgoing = false });
            messagelist.Add(new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = DateTime.Now, IsIncoming = false, IsOutgoing = true });

            Console.WriteLine("im here");
            try
            {
                foreach (var s in App.messearchResponse2)
                {
                    messagelist.Add(new Message
                    {
                        Sender = s.Sender,
                        Reciever = s.Reciever,
                        text = s.Message,
                        TimeStamp = s.TimeStamp,
                        IsIncoming = true,
                        IsOutgoing = false
                    });
                }
            }
            catch
            {

            }
            try
            {
                foreach (var s in App.messearchResponse)
                {
                    {
                        messagelist.Add(new Message
                        {
                            Sender = s.Sender,
                            Reciever = s.Reciever,
                            text = s.Message,
                            TimeStamp = s.TimeStamp,
                            IsIncoming = false,
                            IsOutgoing = true
                        });
                    }
                }
            }
            catch
            {

            }
            return messagelist;
        }
        async Task SaveAsync(MessageDynamo mes)
        {
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            await context.SaveAsync(mes);
        }
    }
}