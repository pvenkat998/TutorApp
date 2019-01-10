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
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Amazon.DynamoDBv2.DocumentModel;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage
    {
        string uppath = "";

        public LoginPage()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            // LoginIcon.Source = Device.RuntimePlatform == Device.Android ? ImageSource.FromFile("LoginIcon.jpg") : ImageSource.FromFile("Images/LoginIcon.jpg");
            //dynamo query test



            LoginIcon.Source = ImageSource.FromResource("LoginIcon.jpg");
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

                path.Text = file.Path;
                uppath = file.Path;
                imgPicked.Source = ImageSource.FromStream(() =>
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
        private async void Takephoto(object sender, EventArgs e)
        {   //camera call
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
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    AllowCropping = true,
                    CompressionQuality = 92,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                path.Text = file.Path;
                uppath = file.Path;

                imgPicked.Source = ImageSource.FromStream(() =>
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

        private void SignIn(object sender, EventArgs e)
        {
            

            // Initialize

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
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            Book retrievedBook = context.LoadAsync<Book>("admin","kanagawa").Result;


            //Enter S3
            AWSConfigsS3.UseSignatureVersion4 = true;
            var s3Client = new AmazonS3Client(App.credentials, App.region);
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

            uprequest.Key = Entry_Username.Text + "_" + "workz.jpg"; //file name up in S3
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG"); //local file name
            uprequest.FilePath = uppath;
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
        public async Task QueryAsync(AWSCredentials credentials, RegionEndpoint region)
        {
            var client = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(client);

            var search = context.FromQueryAsync<Book>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "password",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("Author", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "asd")
            });

            Console.WriteLine("items retrieved");

            var searchResponse = await search.GetRemainingAsync();
            foreach(var s in searchResponse)
            {
                Console.WriteLine(s.ToString());
            }
// searchResponse.ForEach((s) = > {
// Console.WriteLine(s.ToString());});

        }
        void Redirsignup(object sender, EventArgs e)
        {
            QueryAsync(App.credentials, App.region);
            DisplayAlert(Entry_Username.Text, Entry_Password.Text, "w");
            Navigation.PushModalAsync(new Signup());
        }
        
    }


}
