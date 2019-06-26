using Amazon.S3.Transfer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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

    public partial class ShowPost : ContentPage
    {
        string uppath = "";

        public ShowPost()
        {
            App.CurrentPost.PostPicPath = "";
            TransferUtilityDownloadRequest dlreq = new TransferUtilityDownloadRequest();

            // subdirectory and bucket name
            dlreq.BucketName = "tutorapp" + @"/" + "postpics";
            dlreq.Key = App.CurrentPost.UID + "_" + "1.jpg"; //file name up in S3
            dlreq.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.CurrentPost.UID + "_" + "1.jpg"); //local file name
            try
            {
                App.s3utility.DownloadAsync(dlreq).ConfigureAwait(true);
                App.CurrentPost.PostPicPath = dlreq.FilePath;
            }
            catch
            {
                App.CurrentPost.PostPicPath = "";

            }
            BindingContext = App.CurrentPost;
            InitializeComponent();
            if (File.Exists(dlreq.FilePath))
            {
            }
            else
            {
                PostCont.SetValue(Grid.ColumnProperty, 0);
            }
            if (App.CurrentPost.Comments == null)
            {
            }
            else
            {
                List<Comm> worklist = App.CurrentPost.Comments.Where(x => x.ParentCID == null || x.ParentCID == "").ToList();
                int count = worklist.Count();
                List<LRComm> comlist = new List<LRComm>();
                dlreq.BucketName = "tutorapp" + @"/" + "postpics" + @"/"+"commentpics";
                // choose to load all images or just images in comm but not subcomm 
                int i;
                for ( i=0;i< count; i++){
                    dlreq.Key = worklist[i].CID + "_" + "1.jpg"; //file name up in S3
                    dlreq.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), worklist[i].CID + "_" + "1.jpg"); //local file name
                    try
                    {
                        App.s3utility.DownloadAsync(dlreq).ConfigureAwait(true);
                        worklist[i].CommentPicPath = dlreq.FilePath;
                    }
                    catch
                    {
                        worklist[i].CommentPicPath = "";

                    }
                    comlist.Add(new LRComm { CID = worklist[i].CID });
                   // comlist[i].CID = worklist[i].CID;
                    comlist[i].CommentPicPath = worklist[i].CommentPicPath;
                    comlist[i].CommentorName = worklist[i].CommentorName;
                    comlist[i].CommentorEmail = worklist[i].CommentorEmail;
                    comlist[i].Comment = worklist[i].Comment;
                    comlist[i].ParentorChild = worklist[i].ParentorChild;
                    comlist[i].ParentCID = worklist[i].ParentCID;
                    comlist[i].CommentTime = worklist[i].CommentTime;
                    InitializeComponent();
                    if (File.Exists(dlreq.FilePath))
                    {
                        comlist[i].LabCol = 1;
                        comlist[i].PicCol = 0;
                    }
                    else
                    {
                        comlist[i].LabCol = 0;
                        comlist[i].PicCol = 1;
                    }
                }
                Comment.ItemsSource = comlist;
                
            }

        }
        async void Updatecomments(object sender, EventArgs e)
        {
            string x = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            if (App.CurrentPost.Comments == null)
            {
                var com = new List<Comm>();
                com.Add(new Comm { CID = x, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = comment.Text,CommentTime=DateTime.Now });
                App.CurrentPost.Comments = com;
            }
            else
            {
                App.CurrentPost.Comments.Add(new Comm { CID = x, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = comment.Text, CommentTime = DateTime.Now });

            }
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();
            uprequest.BucketName = "tutorapp" + @"/" + "postpics" + @"/" + "commentpics";
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
            await App.context.SaveAsync(App.CurrentPost);
            comment.Text = "";
        }

        async void Updatesubcomments(object sender, EventArgs e)
        {
            var button = sender as Button;
            var te = button.BindingContext as string;


            var buttonClickHandler = (Button)sender;
            // access Parent Layout for Button  
            Grid ParentStackLayout = (Grid)buttonClickHandler.Parent;
            // access first Label "name"  
            Entry Subcommentlabel = (Entry)ParentStackLayout.Children[0];
            string subcomment = Subcommentlabel.Text;
            Console.WriteLine(subcomment);
            string x = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            Console.WriteLine("==1==");
            Console.WriteLine(App.cur_user.email);
            if (App.CurrentPost.Comments == null)
            {
                var com = new List<Comm>();
                com.Add(new Comm { CID = x, ParentCID = te, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = subcomment });
                App.CurrentPost.Comments = com;
            }
            else
            {
                App.CurrentPost.Comments.Add(new Comm { CID = x, ParentCID=te, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = subcomment });

            }
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();
            uprequest.BucketName = "tutorapp" + @"/" + "postpics" + @"/" + "subcommentpics";
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
            await App.context.SaveAsync(App.CurrentPost);
            comment.Text = "";
        }
        async void ShowCommentPage(object sender,EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string CID = eventargs.Parameter.ToString();

            await Navigation.PushModalAsync(new ShowPostComment(CID));

        }
        async void ShowReplies(object sender, EventArgs e)
        {

            var buttonClickHandler = (Button)sender;
            // access Parent Layout for Button  
            Grid ParentStackLayout = (Grid)buttonClickHandler.Parent;
            // access first Label "name"  
            Grid InvisGrid = (Grid)ParentStackLayout.Children[4];
            ListView SubCom = (ListView)ParentStackLayout.Children[5];
            string CID = InvisGrid.BindingContext as string;

            if (InvisGrid.IsVisible) { 
            InvisGrid.IsVisible = false;
                SubCom.IsVisible = false;
            }
            else
            {
                List<Comm> worklist = App.CurrentPost.Comments.Where(x => x.ParentCID == CID).ToList();
                TransferUtilityDownloadRequest dlreq = new TransferUtilityDownloadRequest();
                dlreq.BucketName = "tutorapp" + @"/" + "postpics" + @"/"+"subcommentpics";
                int i;
                for ( i=0;i< worklist.Count();i++){
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), worklist[i].CID + "_" + "1.jpg"));
                    dlreq.Key = worklist[i].CID + "_" + "1.jpg"; //file name up in S3
                    dlreq.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), worklist[i].CID + "_" + "1.jpg"); //local file name
                    try
                    {
                        await App.s3utility.DownloadAsync(dlreq).ConfigureAwait(true);
                        worklist[i].CommentPicPath = dlreq.FilePath;
                        Console.WriteLine("====picdl===");                    }
                    catch
                    {
                        worklist[i].CommentPicPath = "";
                        Console.WriteLine("====nopicdl===");

                    }
                }
                SubCom.ItemsSource = worklist;
                InvisGrid.IsVisible = true;
                if (worklist.Count == 0)
                {
                }
                else
                {
                    SubCom.IsVisible = true;
                }
            }
        }
        private async void ImageselectCom(object sender, EventArgs e)
        {   //gallery call
            uppath = "";
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


        //below is to display comments
    }
}