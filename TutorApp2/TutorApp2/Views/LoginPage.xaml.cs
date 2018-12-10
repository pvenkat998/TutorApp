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
                "ap-northeast-1:c16e10cd-af5b-41c6-9f1b-4eea7605c361", // Identity pool ID
                RegionEndpoint.APNortheast1 // Region
            );

            //MA - -------------  solve ma?----------------------
            analyticsManager = MobileAnalyticsManager.GetOrCreateInstance(
              credentials,
              RegionEndpoint.APNortheast1, // Region
              APP_ID // app id
            );
            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;

            AWSConfigs.AWSRegion = "APNortheast1";
            IAmazonS3 s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);

            string a = "hi";
            string txtSysLog="";
            var cs ="we";
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            string ConnectionString = "server=tutorappmaria.czpzqegto9at.ap-northeast-1.rds.amazonaws.com;port=3306; uid =tutorappmaria;pwd=Asshole!;database=tutorappmaria";
            string ConnectionString2 = "Server=db4free.net;Uid =tutorapp123;Pwd=12345678;Database=tutorapp123;";
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                txtSysLog="there is meaning to live";
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                txtSysLog = ex.ToString();
            }


            DisplayAlert( cs, txtSysLog, a);//do my sql updarte db


        }
        void Redirsignup(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Signup());
        }
    }


}