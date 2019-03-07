using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySql.Data.MySqlClient;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.CognitoIdentity;
using Amazon;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media.Abstractions;
using Amazon.S3;
using Amazon.S3.Transfer;
using System.IO;
using Amazon.S3.Model;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signup : ContentPage
	{
        string uppath="";
        public Signup ()
		{
			InitializeComponent ();
            Init();
        }
        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Email.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Entry_surname.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Entry_Email.Focus();
            Entry_Email.Completed += (s, e) => Entry_station.Focus();
            Entry_station.Completed += (s, e) => Signup1(s, e);

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
        private void WriteFileProgress(object sender, WriteObjectProgressArgs args)
        {
            // show progress
            System.Diagnostics.Debug.WriteLine("=======UpdateFileProgress=======");
        }
        private void Signup1(object sender, EventArgs e)
        {
            var title = "登録完了";
            string textbox = "登録完了";
            string button = "はい！";

            //cred region
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
             "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
                RegionEndpoint.APNortheast1 // Region
            );
            AWSConfigs.AWSRegion = "APNortheast1";
            RegionEndpoint region = RegionEndpoint.APNortheast1;
            var s3Client = new AmazonS3Client(credentials, region);
            var transferUtility = new TransferUtility(s3Client);


            // picture download success
            try
            {

                TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                request.BucketName = "tutorapp" + @"/" + "profilepic";
                request.Key = "default.jpg";
                request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg");
                textbox = request.FilePath;
                TransferUtility tu = new TransferUtility(s3Client);
                request.WriteObjectProgressEvent += WriteFileProgress;

                System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                tu.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
                imgPicked.Source= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg");
            }
            catch (Exception ex)
            {
                textbox = " u bik";
                System.Diagnostics.Debug.WriteLine("=====ERROR ========");
            }
            TransferUtility utility = new TransferUtility(s3Client);
            // making a TransferUtilityUploadRequest instance
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();

            // subdirectory and bucket name
            uprequest.BucketName = "tutorapp" + @"/" + "profilepic";

            uprequest.Key = Entry_Email.Text + "_" + "dp.jpg"; //file name up in S3
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg"); //local file name
            if (uppath != "") { 
            uprequest.FilePath = uppath;
            }

            System.Diagnostics.Debug.WriteLine(uprequest.FilePath);
            System.Diagnostics.Debug.WriteLine("=====IOS1 error========");
            System.Diagnostics.Debug.WriteLine("=====IOS3 error========");
            utility.UploadAsync(uprequest);
            //commensing the transfer
            var dbclient = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(dbclient);

            DateTime now = DateTime.Now.ToLocalTime();
            string text = now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            App.userdata_v1 tosave_info = new App.userdata_v1()
            {
                email = Entry_Email.Text,
                password = Entry_Password.Text,
                stud_teach = Entry_stud_teach.SelectedItem.ToString(),
                surname = Entry_surname.Text,
                gender=Entry_sex.SelectedItem.ToString(),
                age = Entry_age.Text,
                bach_or_mast = Entry_bach_or_mast.SelectedItem.ToString(),
                gakunen = Entry_gakunen.SelectedItem.ToString(),
                karui_major = Entry_karui_major.Text,
                high_school = Entry_high_school.Text,
                chuugaku_juken = Entry_chuugaku_juken.Text,
                shidoukanou = Entry_shidoukanou.Text,
                shidoukeiken = Entry_shidoukeiken.Text,
                hitokoto = Entry_hitokoto.Text,
                station = Entry_station.Text,
                // datetime =  text

            };
            context.SaveAsync(tosave_info);

            var dir =new LoginPage();
            Navigation.PushModalAsync(dir);


        }
    }
}
