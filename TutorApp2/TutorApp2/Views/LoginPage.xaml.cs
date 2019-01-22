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
            LoginIcon.Source = ImageSource.FromResource("LoginIcon.jpg");
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SignIn(s, e);

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
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            App.userdata_v1 retrievedBook;
            retrievedBook = context.LoadAsync<App.userdata_v1>(Entry_Username.Text,Entry_Password.Text).Result;
            App.cur_user.address = retrievedBook.address;

            //Enter S3
            AWSConfigsS3.UseSignatureVersion4 = true;
            var s3Client = new AmazonS3Client(App.credentials, App.region);
            var transferUtility = new TransferUtility(s3Client);

          //  string button = retrievedBook.id.ToString();
            if (retrievedBook!= null) {
                // picture download success
                string textbox = "w";
                string check = "1";
                try
                {

                    TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                    request.BucketName = "tutorapp" + @"/" + "profilepic"; 
                    request.Key = Entry_Username.Text + "_dp.jpg"; 
                    request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "dpuser.jpg");
                    textbox = request.FilePath;
                    TransferUtility tu = new TransferUtility(s3Client);
                    request.WriteObjectProgressEvent += WriteFileProgress;

                    System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                    tu.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
                    App.dp_img_path = request.FilePath;
                }

                catch (Exception ex)
                {
                    textbox = "wololooooo";
                    System.Diagnostics.Debug.WriteLine("=====ERROR ========");
                }
                //
                DisplayAlert("yay", textbox, check);//do my sql updarte db
                Navigation.PushModalAsync(new Home());
                Console.WriteLine("True!");
            }
            else
            {
                path.Text = "user doesnt exist";
                DisplayAlert(Entry_Username.Text, Entry_Password.Text, "ww");//do my sql updarte db
                Console.WriteLine("False!");
            }

        }
        public async Task QueryAsync(AWSCredentials credentials, RegionEndpoint region)
        {
            var client = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(client);

            var search = context.FromQueryAsync<App.userdata_v1>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "password-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("password", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "asd")
               
            });

            Console.WriteLine("items retrieved");

            var searchResponse = await search.GetRemainingAsync();
            foreach(var s in searchResponse)
            {
                Console.WriteLine(s.email.ToString());
            }

        }
        async void Redirsignup(object sender, EventArgs e)
        {
           // await QueryAsync(App.credentials, App.region);
            Navigation.PushModalAsync(new Signup());
        }
        
    }


}
