using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Models;
using System.Data;
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
using Xamarin.Essentials;
using System.Diagnostics;

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
            AutoLogin();
            //test();
        }
        void AutoLogin()
        {
            var properties = Application.Current.Properties;
            if (!properties.ContainsKey("username") && !properties.ContainsKey("password"))
            {
                Console.WriteLine("==aa==");
                properties.Add("username", "");
                properties.Add("password", "");

            }
            else
            {
                Entry_Username.Text = (string)properties["username"];
                if ((string)properties["password"] == "")
                {
                    Entry_Username.Text = (string)properties["username"];
                }
                else
                {
                Entry_Password.Text = (string)properties["password"];
                    LOGIN();
                }
            }
        }
        async void test()
        {
            Xamarin.Essentials.Location userlocation = new Xamarin.Essentials.Location(42.358056, -71.063611);
            Xamarin.Essentials.Location tarlocation = new Xamarin.Essentials.Location(37.783333, -122.416667);

            try
            {
                var useraddress = "渋谷駅";
                var userlocations = await Geocoding.GetLocationsAsync(useraddress);
                userlocation = userlocations?.FirstOrDefault();
                if (userlocation != null)
                {
                    Console.WriteLine($"Latitude: {userlocation.Latitude}, Longitude: {userlocation.Longitude}, Altitude: {userlocation.Altitude}");
                    longi.Text = userlocation.Longitude.ToString();
                    lat.Text = userlocation.Latitude.ToString();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
            try
            {
                var taraddress = "原宿駅";
                var tarlocations = await Geocoding.GetLocationsAsync(taraddress);
                tarlocation = tarlocations?.FirstOrDefault();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
            }
            catch (Exception ex)
            {
            }
            double miles = Xamarin.Essentials.Location.CalculateDistance(userlocation, tarlocation, DistanceUnits.Kilometers);
            dist.Text = miles.ToString();
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
            System.Diagnostics.Debug.WriteLine("=======2=======");

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    AllowCropping = true,
                    CompressionQuality = 92,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                System.Diagnostics.Debug.WriteLine("=======1=======");
                if (file == null)
                    return;

                path.Text = file.Path;
                uppath = file.Path;

                System.Diagnostics.Debug.WriteLine("=======0=======");
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
        async void LOGIN()
        {
            AWSConfigs.AWSRegion = "APNortheast1";

            //MA - -------------  solve ma?----------------------
            //    analyticsManager = MobileAnalyticsManager.GetOrCreateInstance(
            //s credentials,
            App.userdata_v1 retrievedBook;
            //changeeeeee retrievedBook = context.LoadAsync<App.userdata_v1>(Entry_Username.Text,Entry_Password.Text).Result;
            if (Entry_Username.Text == null)
            {
                Entry_Username.Text = "admin@example.com";
            }
            else
            {

            }
            retrievedBook = App.context.LoadAsync<App.userdata_v1>(Entry_Username.Text).Result;
            //       ---------------------------DANGER========================
            //Enter S3
            AWSConfigsS3.UseSignatureVersion4 = true;
            var s3Client = new AmazonS3Client(App.credentials, App.region);
            var transferUtility = new TransferUtility(s3Client);

            //  string button = retrievedBook.id.ToString();
            if (retrievedBook != null)
            {
                // picture download success
                string textbox = "w";
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
                App.cur_user_book = retrievedBook;
                App.cur_user.email = retrievedBook.email;
                App.cur_user.surname = retrievedBook.surname;
                App.cur_user.grade = retrievedBook.edu_tier;
                if (Entry_Password.Text == App.cur_user_book.password)
                {
                    var prop= Application.Current.Properties;
                    prop["username"] = Entry_Username.Text.ToString();
                    prop["password"] = Entry_Password.Text.ToString();
                    await Application.Current.SavePropertiesAsync();
                    System.Diagnostics.Debug.WriteLine("=====HEY========");
                    await Navigation.PushModalAsync(new HomeDetail());
                }
                else
                {
                    
                    path.Text = "wrong password";
                }
            }
            else
            {
                path.Text = "user doesnt exist";
                DisplayAlert(Entry_Username.Text, Entry_Password.Text, "ww");//do my sql updarte db
                Console.WriteLine("False!");
            }
        }
        private void SignIn(object sender, EventArgs e)
        {


            // Initialize
            LOGIN();
            

        }
        
        void Redirsignup(object sender, EventArgs e)
        {
            // await QueryAsync(App.credentials, App.region);
            Navigation.PushModalAsync(new Signup());
        }
        void Profileredir(object sender, EventArgs e)
        {
            // await QueryAsync(App.credentials, App.region);
            App.tarprof = new App.userdata_v1 { };
            App.tarprof.email = "dummy1";
            App.tarprof.password = "nothing here";
            App.tarprof.surname = "kuma";
            App.tarprof.gender = "m";
            App.tarprof.hitokoto = "俺は海賊王になる男だ";
            App.tarprof.gakunen = "1";
            App.tarprof.karui_major = "文科１";
            App.tarprof.high_school = "la salle";
            App.tarprof.bach_or_mast = "大学";
            App.tarprof.chuugaku_juken = "あり";
            App.tarprof.shidoukanou = "3 years";
            App.tarprof.shidoukeiken = "1 year";
            App.tarprof.station = "茗荷谷駅";
            Navigation.PushModalAsync(new ProfilePage());
        }

    }


}
