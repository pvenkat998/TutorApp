using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using Amazon.SQS;
using Amazon.SQS.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        string uppath = "";
        public MessagePageSimple ()
        {
            InitializeComponent();
            List<Message> messagelist =  MessageDB();
            BindingContext = messagelist;
            Debug.WriteLine("=====GGWP ======== ");
            Surname.Text = App.User_Recepient.Username;
            ChosenImg.SizeChanged += (sender, e) => {
                if (ChosenImg.Height > 50)
                    ChosenImg.HeightRequest = 50;
            };
        }
        private async void SendMessage(object sender, EventArgs e)
        {
            Guid x = Guid.NewGuid();
            MessageDynamo mes = new MessageDynamo
            {
                Messageid = x.ToString(),
                Sender = App.cur_user.email,
                Reciever = App.User_Recepient.Email,
                Message = Messagetosend.Text,
                TimeStamp = DateTime.Now,
                RecieverName = App.User_Recepient.Username
            };
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();
            uprequest.BucketName = "tutorapp" + @"/" + "messagepic";
            uprequest.Key = x + "_" + "1.jpg"; //file name up in S3
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg"); //local file name
            if (uppath != "")
            {
                uprequest.FilePath = uppath;
                await App.s3utility.UploadAsync(uprequest);
            }
            await SaveAsync(mes);

            msg.ItemsSource  = null;
            List<Message> messagelist = MessageDB();
            msg.ItemsSource = messagelist;
            Messagetosend.Text = "";
        }
        public List<Message> MessageDB()
        {
            App.User_Recepient.PicSrc= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.User_Recepient.Email+"_dp.jpg"); //local file name
            if (File.Exists(App.User_Recepient.PicSrc))
            {
            }
            else
            {
                TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                request.BucketName = "tutorapp" + @"/" + "profilepic";
                request.Key = App.User_Recepient.Email+"_dp.jpg";
                request.FilePath = App.User_Recepient.PicSrc;
                try
                {
                    App.s3utility.DownloadAsync(request).ConfigureAwait(true);

                }
                catch
                {
                    Console.WriteLine("===navprof==");
                }
            }
            App.MessageID.Clear();
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
                    bool pic = false;
                    bool text = true;
                    if (s.Message == s.Messageid)
                    {
                        text = false;
                    }
                    string mps = "";
                    TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                    request.BucketName = "tutorapp" + @"/" + "messagepic";
                    request.Key = s.Messageid.ToString() + "_1.jpg";
                    request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), s.Messageid.ToString() + "_1.jpg");
                    try
                    {
                        App.s3utility.DownloadAsync(request).ConfigureAwait(true);
                        mps = request.FilePath;
                        if (File.Exists(mps)) { 
                        pic = true;
                        text = false;
                        App.MessageID.Add(mps);
                        }
                    }
                    catch { }
                    messagelist.Add(new Message
                    {
                        Sender = s.Sender,
                        Reciever = s.Reciever,
                        Text = s.Message,
                        TimeStamp = s.TimeStamp,
                        IsIncoming = true,
                        IsOutgoing = false,
                        Rec_ImageSrc = App.User_Recepient.PicSrc,
                        MsgPicSrc = mps,
                        IsPic = pic,
                        IsText = text
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
                    
                    bool pic = false;
                    bool text = true;
                    if (s.Message == s.Messageid)
                    {
                        text = false;
                    }
                    string mps = "";
                    TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                    request.BucketName = "tutorapp" + @"/" + "messagepic";
                    request.Key = s.Messageid.ToString() + "_1.jpg";
                    request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), s.Messageid.ToString() + "_1.jpg");
                    try
                    {
                        App.s3utility.DownloadAsync(request).ConfigureAwait(true);
                        mps = request.FilePath;
                        if (File.Exists(mps))
                        { pic = true;
                            text = false;
                            App.MessageID.Add(mps);
                        }

                    }
                    catch { }
                    messagelist.Add(new Message
                    {
                        Sender = s.Sender,
                        Reciever = s.Reciever,
                        Text = s.Message,
                        TimeStamp = s.TimeStamp,
                        IsIncoming = false,
                        IsOutgoing = true,
                        Rec_ImageSrc = App.User_Recepient.PicSrc,
                        MsgPicSrc = mps,
                        IsPic = pic,
                        IsText = text
                    });
                    
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
        private async void Imageselect(object sender, EventArgs e)
        {   //gallery call
            await CrossMedia.Current.Initialize();
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    CompressionQuality = 92
                });

                if (file == null)
                    return;

                uppath = file.Path;
                ChosenImg.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();
            }

        }
        async void Button2(object sender,EventArgs e)
        {
            await Navigation.PushModalAsync(new ChatImages());
        }
        async void Buttonse(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ReportList());

        }
        async void Profrdr(object sender, EventArgs e)
        {
            try { 
            App.tarprof = App.searchResponse.Single(r => r.email == App.User_Recepient.Email);
            }
            catch
            {
                App.userdata_v1 retrievedBook;
                retrievedBook = App.context.LoadAsync<App.userdata_v1>(App.User_Recepient.Email).Result;
                App.tarprof = retrievedBook;
            }
            await Navigation.PushModalAsync(new ProfilePage());
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Navigation.PushModalAsync(new HomeDetail2());
            return true;
        }
    }
}