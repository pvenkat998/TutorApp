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
    public partial class ShowPostComment : ContentPage
    {
        public ShowPostComment(string CID)
        {
            Comm curcom = App.CurrentPost.Comments.Where(x  => x.CID == CID).ToList()[0];
            BindingContext = curcom;
            InitializeComponent();
            List<Comm> worklist = App.CurrentPost.Comments.Where(x => x.ParentCID == CID).ToList();
            TransferUtilityDownloadRequest dlreq = new TransferUtilityDownloadRequest();
            dlreq.BucketName = "tutorapp" + @"/" + "postpics" + @"/" + "subcommentpics";
            int i;
            for (i = 0; i < worklist.Count(); i++)
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), worklist[i].CID + "_" + "1.jpg"));
                dlreq.Key = worklist[i].CID + "_" + "1.jpg"; //file name up in S3
                dlreq.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), worklist[i].CID + "_" + "1.jpg"); //local file name
                try
                {
                    App.s3utility.DownloadAsync(dlreq).ConfigureAwait(true);
                    worklist[i].CommentPicPath = dlreq.FilePath;
                    Console.WriteLine("====picdl===");
                }
                catch
                {
                    worklist[i].CommentPicPath = "";
                    Console.WriteLine("====nopicdl===");

                }
            }
            Comment.ItemsSource = worklist;
        }
    }
}