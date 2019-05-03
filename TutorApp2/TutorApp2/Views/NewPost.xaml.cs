using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3.Transfer;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPost : ContentPage
    {
        string uppath = "";
        public NewPost ()
		{
            InitializeComponent ();
        }
        void Back(object sender,EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        async void NewPost1(object sender,EventArgs e)
        {
            Guid x= Guid.NewGuid();
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();

            // subdirectory and bucket name
            uprequest.BucketName = "tutorapp" + @"/" + "postpics";
            uprequest.Key = x + "_" + "1.jpg"; //file name up in S3
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg"); //local file name
            if (uppath != "")
            {
                uprequest.FilePath = uppath;
                App.s3utility.UploadAsync(uprequest);
            }
            Post NP = new Post()
            {
                UID = x.ToString(),
                Title = PostTitle.Text,
                PosterEmail = App.cur_user.email,
                PosterName=App.cur_user.surname,
                Grade=App.cur_user.grade,
                Content=PostCont.Text,
                Likes="0",
                Comments = null,
                PostTime=DateTime.Now,
                PostType="Post"
            };
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            await context.SaveAsync(NP);
            await Navigation.PushModalAsync(new Forum());

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
                image.Source = ImageSource.FromStream(() =>
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

        public async void Takephoto(object sender, EventArgs e)
        {   //camera call screen.
            CrossPermissions.Current.OpenAppSettings();
            System.Diagnostics.Debug.WriteLine("=======0=======");
            await CrossMedia.Current.Initialize();

            System.Diagnostics.Debug.WriteLine("=======1=======");
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            System.Diagnostics.Debug.WriteLine("=======2=======");

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            else if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
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

                image.Source = ImageSource.FromStream(() =>
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
    }
}