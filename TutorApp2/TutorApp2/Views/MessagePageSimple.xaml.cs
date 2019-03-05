using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Models
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessagePageSimple : ContentPage
    {
        List<Message> messagelist = new List<Message>();
        public MessagePageSimple ()
        {

            messagelist.Add(new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = new DateTime(2008, 5, 1, 8, 30, 52), IsIncoming = true ,IsOutgoing= false});
            messagelist.Add(new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = DateTime.Now, IsIncoming = false, IsOutgoing=true });
            string sender = "a";
            string reciever = "a";


            foreach (var s in App.messearchResponse)
            {
                if (s.Sender == "a") { 
                messagelist.Add(new Message
                {
                    Sender = s.Sender , Reciever=s.Reciever,text=s.Message,TimeStamp=s.TimeStamp,IsIncoming=true,IsOutgoing=false
                });
                }
                else if(s.Reciever=="a")
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

            BindingContext = messagelist;
            InitializeComponent ();
		}



        private async void SendMessage(object sender, EventArgs e)
        {

            MessageDynamo mes = new MessageDynamo
            {
                Messageid= Guid.NewGuid().ToString(),
                Sender = "a",
                Reciever = "a",
                Message = Messagetosend.Text,
                TimeStamp = DateTime.Now
            };

            Console.WriteLine("aa");
            await SaveAsync(mes);
        }
        async Task SaveAsync(MessageDynamo mes)
        {
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            await context.SaveAsync(mes);
        }
    }
}