using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Views;
using Amazon.CognitoIdentity;
using Amazon;
using Amazon.DynamoDBv2.DataModel;

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
        [DynamoDBTable("registered_userdata")]
        public class registered_userdata
        {
            [DynamoDBHashKey]    // Hash key.
            public string email { get; set; }
            [DynamoDBRangeKey]
            public string add_ku_sort { get; set; }
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            //  public int Price { get; set; }
            public string address { get; set; }
            //  public string datetime { get; set; }
        }
        public static Image dp_img;
        public static string dp_img_path;
        public static class cur_user
        {
            public static string email { get; set; }
            public static string add_ku_sort { get; set; }
            public static int id { get; set; }
            public static string username { get; set; }
            public static string password { get; set; }
            public static string address { get; set; }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
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
