using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Views;
using Amazon.CognitoIdentity;
using Amazon;
using Amazon.DynamoDBv2.DataModel;
using TutorApp2.Models;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using System.Diagnostics;
using Amazon.S3.Transfer;
using Amazon.S3;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TutorApp2
{
    public partial class App : Application
    {
        public static CognitoAWSCredentials credentials = new CognitoAWSCredentials(
    "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
       RegionEndpoint.APNortheast1 // Region
   );
        public static RegionEndpoint region = RegionEndpoint.APNortheast1;
        [DynamoDBTable("userdata_v1")]
        public class userdata_v1
        {
            [DynamoDBHashKey]    // Hash key.
            public string email { get; set; }
            public string password { get; set; }
            public string stud_teach { get; set; }
            public string surname { get; set; }
            public string gender { get; set; }
            public string age { get; set; }
            public string bach_or_mast { get; set; }
            public string gakunen { get; set; }
            public string karui_major { get; set; }
            public string edu_tier { get; set; }
            public string high_school { get; set; }
            public string chuugaku_juken { get; set; }
            public string shidoukanou { get; set; }
            public string shidoukeiken { get; set; }
            public string hitokoto { get; set; }
            public string station { get; set; }
        }
        public static List<string> MessageID = new List<string>();
        public static AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, region);
        public static DynamoDBContext context = new DynamoDBContext(new AmazonDynamoDBClient(credentials, region));
        public static TransferUtility s3utility = new TransferUtility(new AmazonS3Client(credentials, region));
        public static List<Post> QueriedPosts;
        public static Post CurrentPost;
        public static Image dp_img;
        public static string dp_img_path;
        public static List<MessageDynamo> messearchResponse;
        public static List<MessageDynamo> messearchResponse2;
        public static List<Message> AllMessageList;
        public static List<userdata_v1> searchResponse;
        public static userdata_v1 tarprof = new App.userdata_v1 {email="" };
        public static userdata_v1 cur_user_book;
        public static class cur_user
        {
            public static string email { get; set; }
            public static string surname { get; set; }
            public static string username { get; set; }
            public static string grade { get; set; }
        }
        public static class User_Recepient
        {
            public static string Email { get; set; }
            public static string Username { get; set; }
            public static string PicSrc { get; set; }
        }
        public App()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo((Environment.GetFolderPath(Environment.SpecialFolder.Personal)));

            foreach (FileInfo file in di.GetFiles())
            {
               // file.Delete();
            }
            InitializeComponent();

           // MainPage = new LoginPage();
            App.cur_user.email = "admin@example.com";
            App.User_Recepient.Email = "dummy3@example.com";
            App.User_Recepient.PicSrc = "TutorApp2/Images/kuma.jpg";
            App.User_Recepient.Username = "Nub";
            MainPage = new MessagePageSimple();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
