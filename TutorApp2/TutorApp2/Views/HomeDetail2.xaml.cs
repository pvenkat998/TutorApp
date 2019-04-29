using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomeDetail2 : ContentPage
	{
		public HomeDetail2 ()
        {
            BindingContext = RecieverDB();
            InitializeComponent();
            b1.Source = ImageSource.FromResource("TutorApp2.Images.Searchicon.png");
            b2.Source = ImageSource.FromResource("TutorApp2.Images.Mailicon.png");
            b3.Source = ImageSource.FromResource("TutorApp2.Images.Forumicon.png");
            b4.Source = ImageSource.FromResource("TutorApp2.Images.Profileicon.png");
        }
        void b1c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail());
        }
        void b2c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail2());
        }
        void b3c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Forum());
        }
        void b4c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ProfilePage());
        }
        public class Recievers
        {
            public string Reciever { get; set; }
            public string Reciever_Surname { get; set; }
            public string Text { get; set; }
            public DateTime Timestamp { get; set; }
            public string Rec_ImageSrc { get; set; }
        }
        public List<Recievers> RecieverDB()
        {
            QueryFilter filter = new QueryFilter();
            filter.AddCondition("Sender", QueryOperator.Equal, App.cur_user.email);
            var client = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(client);
            var searchm = context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Sender-Reciever-index",
                Filter = filter
            });
            Console.WriteLine("bb items retrieved");
            App.messearchResponse = searchm.GetRemainingAsync().Result;
            QueryFilter filter2 = new QueryFilter();
            filter2.AddCondition("Reciever", QueryOperator.Equal, App.cur_user.email);
            var searchm2 = context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Reciever-Sender-index",
                Filter = filter2

            });
            Console.WriteLine(App.messearchResponse);
            App.messearchResponse2 = searchm2.GetRemainingAsync().Result;
            //messearchResponse2 sender is diff
            //messearchResponse  reciever is diff
            //List<Message> messagelist = new List<Message>();
            List<Recievers> recievers = new List<Recievers>();
            Console.WriteLine("im here");
            try
            {
                foreach (var s in App.messearchResponse2)
                {
                    if (recievers.Any(x => x.Reciever == s.Sender && x.Timestamp>s.TimeStamp))
                    {
                        
                    }
                    if (recievers.Any(x => x.Reciever == s.Sender && x.Timestamp <= s.TimeStamp))
                    {
                        var itemToRemove = recievers.Single(r => r.Reciever == s.Sender);
                        recievers.Remove(itemToRemove);
                        recievers.Add(new Recievers() {Reciever=s.Sender, Text=s.Message,Timestamp=s.TimeStamp });

                    }
                    else
                    {
                        recievers.Add(new Recievers() { Reciever = s.Sender, Text = s.Message, Timestamp = s.TimeStamp});

                    }
                }
            }
            catch
            {

            }
            try
            {
                foreach (var s in App.messearchResponse)
                {
                    if (recievers.Any(x => x.Reciever == s.Reciever && x.Timestamp > s.TimeStamp))
                    {

                    }
                    else if (recievers.Any(x => x.Reciever == s.Reciever && x.Timestamp <= s.TimeStamp))
                    {
                        var itemToRemove = recievers.Single(r => r.Reciever == s.Reciever);
                        recievers.Remove(itemToRemove);
                        recievers.Add(new Recievers() { Reciever = s.Reciever, Text = s.Message, Timestamp = s.TimeStamp});

                    }
                    else
                    {
                        recievers.Add(new Recievers() { Reciever = s.Reciever, Text = s.Message, Timestamp = s.TimeStamp});

                    }
                }
            }
            catch
            {

            }
            for (int i=0;i<recievers.Count;i++)
            {
                try
                {
                    App.userdata_v1 k = (App.searchResponse.Single(x => x.email == recievers[i].Reciever));
                    recievers[i].Reciever_Surname = k.surname;
                    Console.WriteLine("db exists");
                }
                catch
                {
                    App.userdata_v1 retrievedBook;
                    retrievedBook = App.context.LoadAsync<App.userdata_v1>(recievers[i].Reciever).Result;
                    recievers[i].Reciever_Surname = "ERROR_fix in HomeDetail2";
                }
                Console.WriteLine(recievers[i].Reciever);
                string imgpath=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), recievers[i].Reciever + "_dp.jpg");
                if (File.Exists(imgpath))
                {
                    Console.WriteLine("file exists");
                    recievers[i].Rec_ImageSrc = imgpath;

                }
                else
                {
                    Console.WriteLine("file downloading");
                    try
                    {

                        TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                        request.BucketName = "tutorapp" + @"/" + "profilepic";
                        request.Key = recievers[i].Reciever + "_dp.jpg";
                        request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), recievers[i].Reciever + "_dp.jpg");

                        System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                        App.s3utility.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
                        recievers[i].Rec_ImageSrc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), recievers[i].Reciever + "_dp.jpg");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("=====ERROR ========");
                    }
                }
            }
            List<Recievers> SortedList = recievers.OrderByDescending(o => o.Timestamp).ToList();
            return SortedList;
        }
        private async void OnTapped2(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
           
            App.User_Recepient.Email = te;
            App.User_Recepient.Username = "sender";
            App.User_Recepient.PicSrc= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), te + "_dp.jpg");
            await Navigation.PushModalAsync(new MessagePageSimple());
            
        }
        private async void onhold(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
            var action = await DisplayActionSheet("アクション", "戻る", null, "プロフィールをみる", "メッセージする", "通報する");
            if (action == "プロフィールをみる")
            {
                App.User_Recepient.Email = te;
                await Navigation.PushModalAsync(new ProfilePage());
            }
            if (action == "メッセージする")
            {
                App.User_Recepient.Email = te;
                App.User_Recepient.Username = "vv";

                await Navigation.PushModalAsync(new MessagePageSimple());
            }
            if (action == "通報する")
            {
                await DisplayAlert("通報できた", "通報できた", te);//do my sql updarte db

            }
            Debug.WriteLine("Action: " + action);
        }
    }
}