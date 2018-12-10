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
using Amazon.MobileAnalytics.MobileAnalyticsManager;
using Amazon.MobileAnalytics;
using Amazon.S3.Transfer;
using System.IO;
namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            System.Diagnostics.Debug.WriteLine("test1");

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


            User user = new User(Entry_Username.Text, Entry_Password.Text);

            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
                "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
                RegionEndpoint.APNortheast1 // Region
            );


           // IAmazonS3 s3Client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1);

            //MA - -------------  solve ma?----------------------

            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;

            AWSConfigsS3.UseSignatureVersion4 = true;
            var s3Client = new AmazonS3Client(credentials, RegionEndpoint.APNortheast1);
            var transferUtility = new TransferUtility(s3Client);
            System.Diagnostics.Debug.WriteLine("test2");
        //    transferUtility.Upload("C:/Users/VENKAT GOD/AppData/Roaming/test.pdf", "tutorapp","12");

            string a = "hi";
            string txtSysLog="";
            var cs ="we";



            DisplayAlert( cs, txtSysLog, a);//do my sql updarte db


        }
        void Redirsignup(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Signup());
        }
    }


}