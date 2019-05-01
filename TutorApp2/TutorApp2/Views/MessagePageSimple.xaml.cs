using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using TutorApp2.Views;
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
            Debug.WriteLine("=====GGWP ======== ");
            InitializeComponent();
            Surname.Text = App.User_Recepient.Username;
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
            // message partner is App.User_Recepient.Email
            // cur user in from App.cur_user.email
            filter.AddCondition("Sender", QueryOperator.Equal, App.cur_user.email);
            filter.AddCondition("Reciever", QueryOperator.Equal, App.User_Recepient.Email);
            var searchm = App.context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Sender-Reciever-index",
                Filter=filter
            });
            Console.WriteLine("bb items retrieved");
            App.messearchResponse = searchm.GetRemainingAsync().Result;
            QueryFilter filter2 = new QueryFilter();
            filter2.AddCondition("Reciever", QueryOperator.Equal, App.cur_user.email);
            filter2.AddCondition("Sender", QueryOperator.Equal, App.User_Recepient.Email);
            var searchm2 = App.context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Reciever-Sender-index",
                Filter = filter2

            });
            Console.WriteLine(App.messearchResponse);
            App.messearchResponse2 = searchm2.GetRemainingAsync().Result;
            //working with list
            List<Message> messagelist = new List<Message>();
            messagelist.Add(new Message { Sender = "s", Reciever = "r", Text = "msdadss", TimeStamp = new DateTime(2008, 5, 1, 8, 30, 52), IsIncoming = true, IsOutgoing = false, Rec_ImageSrc = App.User_Recepient.PicSrc });
            messagelist.Add(new Message { Sender = "s", Reciever = "r", Text = "mes", TimeStamp = DateTime.Now, IsIncoming = false, IsOutgoing = true });

            Console.WriteLine("im here");
            try
            {
                foreach (var s in App.messearchResponse2)
                {
                    messagelist.Add(new Message
                    {
                        Sender = s.Sender,
                        Reciever = s.Reciever,
                        Text = s.Message,
                        TimeStamp = s.TimeStamp,
                        IsIncoming = true,
                        IsOutgoing = false,
                        Rec_ImageSrc = App.User_Recepient.PicSrc
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
                            Text = s.Message,
                            TimeStamp = s.TimeStamp,
                            IsIncoming = false,
                            IsOutgoing = true,
                            Rec_ImageSrc = App.User_Recepient.PicSrc
                        });
                    }
                }
            }
            catch
            {

            }

            List<Message> SortedList = messagelist.OrderBy(o => o.TimeStamp).ToList();
            return SortedList;
        }
        async Task SaveAsync(MessageDynamo mes)
        {
            
            await App.context.SaveAsync(mes);
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Navigation.PushModalAsync(new HomeDetail2());
            return true;
        }
    }
}