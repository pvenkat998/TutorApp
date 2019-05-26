using Amazon.S3.Transfer;
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
            if (App.CurrentPost.Comments == null)
            {
            }
            else
            {
                Comment.ItemsSource = App.CurrentPost.Comments.Where(x => x.ParentCID == null || x.ParentCID=="" ).ToList();
            }
        }
        async void Updatecomments(object sender, EventArgs e)
        {
            string x = Guid.NewGuid().ToString();
            if (App.CurrentPost.Comments == null)
            {
                var com = new List<Comm>();
                com.Add(new Comm { CID = x, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = comment.Text });
                App.CurrentPost.Comments = com;
            }
            else
            {
                App.CurrentPost.Comments.Add(new Comm { CID = x, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = comment.Text });

            }
            await App.context.SaveAsync(App.CurrentPost);
            comment.Text = "";
            Commentstatus.Text = "";
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
            string x = Guid.NewGuid().ToString();
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
            Console.WriteLine("==5==");
            await App.context.SaveAsync(App.CurrentPost);
            comment.Text = "";
            Commentstatus.Text = "";
        }
        void ShowReplies(object sender, EventArgs e)
        {

            var buttonClickHandler = (Button)sender;
            // access Parent Layout for Button  
            Grid ParentStackLayout = (Grid)buttonClickHandler.Parent;
            // access first Label "name"  
            Grid InvisGrid = (Grid)ParentStackLayout.Children[3];
            ListView SubCom = (ListView)ParentStackLayout.Children[4];
            string CID = InvisGrid.BindingContext as string;

            if (InvisGrid.IsVisible) { 
            InvisGrid.IsVisible = false;
                SubCom.IsVisible = false;
            }
            else
            {
                List<Comm> comlist = App.CurrentPost.Comments.Where(x => x.ParentCID == CID).ToList();
                SubCom.ItemsSource = comlist;
                InvisGrid.IsVisible = true;
                if (comlist.Count == 0)
                {
                }
                else
                {
                    SubCom.IsVisible = true;
                }
            }
            //below is to display comments

        }
    }
}