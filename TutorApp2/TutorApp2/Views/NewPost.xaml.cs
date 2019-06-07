﻿using Amazon.DynamoDBv2;
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
            Btn_imageselect.Source= ImageSource.FromResource("TutorApp2.Images.choosepic.png");
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
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), x + "_" + "1.jpg"); //local file name
            try
            {
                uprequest.FilePath = uppath;
                await App.s3utility.UploadAsync(uprequest);
            }
            catch
            {
                Console.WriteLine(x + "no pic");
            }
            Post NP = new Post()
            {
                UID = x.ToString(),
                Title = PostTitleS.SelectedItem.ToString()+ PostTitleY.SelectedItem.ToString() + PostTitleT.SelectedItem.ToString(),
                PosterEmail = App.cur_user.email,
                PosterName=App.cur_user.surname,
                Grade=App.cur_user.grade,
                Content=PostCont.Text,
                Comments = null,
                PostTime=DateTime.Now,
                PostType="Post"
            };
            await App.context.SaveAsync(NP);
            await Navigation.PushModalAsync(new Forum());

        }
        void ElemTill6(object sender,EventArgs e)
        {
            List<string> ShouList = new List<string>();
            ShouList.Add("1");
            ShouList.Add("2");
            ShouList.Add("3");
            ShouList.Add("4");
            ShouList.Add("5");
            ShouList.Add("6");
            List<string> RemList = new List<string>();
            RemList.Add("1");
            RemList.Add("2");
            RemList.Add("3");
            if (PostTitleS.SelectedIndex == 0) {
                PostTitleY.ItemsSource = ShouList;
            }
            else if (PostTitleS.SelectedIndex > 0)
            {

                PostTitleY.ItemsSource = RemList;
            }
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