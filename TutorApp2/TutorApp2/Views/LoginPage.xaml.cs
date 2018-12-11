using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Models;
using System.Data;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Amazon;
using Amazon.S3;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DynamoDBTable("User_info")]
    public class User_info
    {
        [DynamoDBHashKey]    // Hash key.
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
      //  public int Price { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }

    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SignIn(s, e);

        }
        
        private void SignIn(object sender, EventArgs e)
        {
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
             "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
                RegionEndpoint.APNortheast1 // Region
            );

            //MA - -------------  solve ma?----------------------
        //    analyticsManager = MobileAnalyticsManager.GetOrCreateInstance(
             //s credentials,
             // RegionEndpoint.APNortheast1, // Region
       //       APP_ID // app id
            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;
            RegionEndpoint region = RegionEndpoint.APNortheast1;
            AWSConfigs.AWSRegion = "APNortheast1";
            IAmazonS3 s3Client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1);

            var dbclient = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            User_info songOfIceAndFire = new User_info()
            {
                Id = 1,
                username = "Game Of Thrones",
                password = "978-0553593716",
                email = "819@gmail.com",
                address = "GRRM"
            };
            context.SaveAsync(songOfIceAndFire);  //change Save to SaveAsync
            
            string a = "hi";
            string txtSysLog="";
            var cs ="we";
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            DisplayAlert( cs, txtSysLog, a);//do my sql updarte db


        }
        void Redirsignup(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Signup());
        }
    }


}