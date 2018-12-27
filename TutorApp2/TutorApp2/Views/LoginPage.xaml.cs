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
using Amazon.S3.Model;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.SecurityToken;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using System.IO;
using System.Threading;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using XPA_PickMedia_XLabs_XFP;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
        CameraViewModel cameraOps = null;
        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            // LoginIcon.Source = Device.RuntimePlatform == Device.Android ? ImageSource.FromFile("LoginIcon.jpg") : ImageSource.FromFile("Images/LoginIcon.jpg");
            cameraOps = new CameraViewModel();


            LoginIcon.Source = ImageSource.FromResource("TutorApp2.Images.LoginIcon.jpg");
            Lbl_Username.Text = "why always me?";
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SignIn(s, e);

        }
        [DynamoDBTable("registered_userdata")]
        public class Book
        {
            [DynamoDBHashKey]    // Hash key.
            public string email { get; set; }
            public string add_ku_sort { get; set; }
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            //  public int Price { get; set; }
            public string address { get; set; }
            //  public string datetime { get; set; }
        }
        private void WriteFileProgress(object sender, WriteObjectProgressArgs args)
        {
            // show progress
            System.Diagnostics.Debug.WriteLine("=======UpdateFileProgress=======");
        }
        public interface IFileAccess
        {
            bool Exists(string filename);
            string FullPath(string filename);
            void WriteStream(string filename, Stream streamIn);
        }
        public class FileAccess : IFileAccess
        {
            public bool Exists(string filename)
            {
                var filePath = GetFilePath(filename);

                if (File.Exists(filePath))
                {
                    FileInfo finf = new FileInfo(filePath);
                    return finf.Length > 0;
                }
                else
                    return false;
            }

            public string FullPath(string filename)
            {
                var filePath = GetFilePath(filename);
                return filePath;
            }

            static string GetFilePath(string filename)
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                return filePath;
            }

            public void WriteStream(string filename, Stream streamIn)
            {
                var filePath = GetFilePath(filename);
                using (var fs = File.Create(filePath))
                {
                    streamIn.CopyTo(fs);
                }
            }
        }
        private async void Imageselect(object sender, EventArgs e)
        {   //camera call

            CameraViewModel cameraOps = new CameraViewModel();
            await cameraOps.SelectPicture();
            imgPicked.Source = cameraOps.ImageSource;
            entDetails.Text = "";
        }


        private void SignIn(object sender, EventArgs e)
        {
            

            // Initialize
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
             "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
                RegionEndpoint.APNortheast1 // Region
            );
            RegionEndpoint region = RegionEndpoint.APNortheast1;
            AWSConfigs.AWSRegion = "APNortheast1";

            //MA - -------------  solve ma?----------------------
            //    analyticsManager = MobileAnalyticsManager.GetOrCreateInstance(
            //s credentials,
            // RegionEndpoint.APNortheast1, // Region
            //       APP_ID // app id

            //Enter Dynamo
            var loggingConfig = AWSConfigs.LoggingConfig;
            loggingConfig.LogMetrics = true;
            loggingConfig.LogResponses = ResponseLoggingOption.Always;
            loggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;
            loggingConfig.LogTo = LoggingOptions.SystemDiagnostics;
            //Basic getinfo
            var dbclient = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            Book retrievedBook = context.LoadAsync<Book>("admin","kanagawa").Result;


            //Enter S3
            AWSConfigsS3.UseSignatureVersion4 = true;
            var s3Client = new AmazonS3Client(credentials, region);
            var transferUtility = new TransferUtility(s3Client);


            // picture download success
            string textbox = "w";
            try
            {

                TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                request.BucketName = "tutorapp";
                request.Key = "Capture.PNG";
                request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG");
                textbox = request.FilePath;
                TransferUtility tu = new TransferUtility(s3Client);
                request.WriteObjectProgressEvent += WriteFileProgress;

                System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                tu.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                textbox = "fuck u bik";
                System.Diagnostics.Debug.WriteLine("=====ERROR ========");
            }
            // picture upload trial 

            TransferUtility utility = new TransferUtility(s3Client);
            // making a TransferUtilityUploadRequest instance
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();

            // subdirectory and bucket name
            uprequest.BucketName = "tutorapp" + @"/" + "profilepic";

            uprequest.Key = Entry_Username.Text + "_" + "pp1.jpg"; //file name up in S3
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG"); //local file name
            utility.UploadAsync(uprequest); //commensing the transfer

            // https://www.codeproject.com/Articles/186132/Beginning-with-Amazon-S3
            string button = retrievedBook.id.ToString();
            if (Entry_Username.Text=="admin"&&Entry_Password.Text=="admin") {
                DisplayAlert("yay", textbox, button);//do my sql updarte db
                Navigation.PushModalAsync(new Home());
                Console.WriteLine("True!");
            }
            else
            {
                DisplayAlert(Entry_Username.Text, Entry_Password.Text, button);//do my sql updarte db
                Console.WriteLine("False!");
            }

        }
        void Redirsignup(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Signup());
        }
        
    }


}
