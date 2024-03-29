﻿using Amazon.DynamoDBv2;
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
        public static List<Message> RecieverDB()
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
            List<Message> recievers = new List<Message>();
            Console.WriteLine("im here");
            try
            {
                foreach (var s in App.messearchResponse2)
                {
                    string ColorBox = "White";
                    if (s.Message == s.Messageid)
                    {
                        ColorBox = "LightBlue";
                    }
                    if (recievers.Any(x => x.Reciever == s.Sender && x.TimeStamp > s.TimeStamp))
                    {

                    }
                    if (recievers.Any(x => x.Reciever == s.Sender && x.TimeStamp <= s.TimeStamp))
                    {
                        var itemToRemove = recievers.Single(r => r.Reciever == s.Sender);
                        recievers.Remove(itemToRemove);
                        recievers.Add(new Message() { Reciever = s.Sender, Text = s.Message, TimeStamp = s.TimeStamp, Reciever_Surname = s.RecieverName, Color = ColorBox });

                    }
                    else
                    {
                        recievers.Add(new Message() { Reciever = s.Sender, Text = s.Message, TimeStamp = s.TimeStamp, Reciever_Surname = s.RecieverName, Color = ColorBox });

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
                    string ColorBox = "White";
                    if (s.Message == s.Messageid)
                    {
                        ColorBox = "LightBlue";
                    }
                    if (recievers.Any(x => x.Reciever == s.Reciever && x.TimeStamp > s.TimeStamp))
                    {

                    }
                    else if (recievers.Any(x => x.Reciever == s.Reciever && x.TimeStamp <= s.TimeStamp))
                    {
                        var itemToRemove = recievers.Single(r => r.Reciever == s.Reciever);
                        recievers.Remove(itemToRemove);
                        recievers.Add(new Message() { Reciever = s.Reciever, Text = s.Message, TimeStamp = s.TimeStamp, Reciever_Surname = s.RecieverName, Color = ColorBox });

                    }
                    else
                    {
                        recievers.Add(new Message() { Reciever = s.Reciever, Text = s.Message, TimeStamp = s.TimeStamp, Reciever_Surname = s.RecieverName, Color = ColorBox });

                    }
                }
            }
            catch
            {

            }
            for (int i = 0; i < recievers.Count; i++)
            {
                Console.WriteLine(recievers[i].Reciever);
                string imgpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), recievers[i].Reciever + "_dp.jpg");
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
            App.SortedList = recievers.OrderByDescending(o => o.TimeStamp).ToList();
            return App.SortedList;
        }
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
            App.tarprof.email = App.cur_user.email;
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
        
        private async void OnTapped2(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
           
            App.User_Recepient.Email = te;
            try
            {
                App.userdata_v1 k = (App.searchResponse.Single(x => x.email == te));
                App.User_Recepient.Username = k.surname;
                App.User_Recepient.Grade = k.edu_tier;
                Console.WriteLine("db exists");
            }
            catch
            {
                App.userdata_v1 retrievedBook;
                retrievedBook = App.context.LoadAsync<App.userdata_v1>(te).Result;

                //App.userdata_v1 k = (App.searchResponse.Single(x => x.email == recievers[i].Reciever));
                App.User_Recepient.Username = retrievedBook.surname;
                App.User_Recepient.Grade = retrievedBook.edu_tier;
            }
            App.User_Recepient.PicSrc= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), te + "_dp.jpg");
            await Navigation.PushModalAsync(new MessagePageSimple());
            
        }
    }
}