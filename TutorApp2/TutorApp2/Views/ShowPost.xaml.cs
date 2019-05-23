using System;
using System.Collections.Generic;
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
            BindingContext = App.CurrentPost;
            InitializeComponent();
            if (App.CurrentPost.Comments == null)
            {
            }
            else
            {
                Comment.ItemsSource = App.CurrentPost.Comments;
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
            TappedEventArgs eventargs = e as TappedEventArgs;
            string te = eventargs.Parameter.ToString();
            var buttonClickHandler = (Button)sender;
            // access Parent Layout for Button  
            Grid ParentStackLayout = (Grid)buttonClickHandler.Parent;
            // access first Label "name"  
            Entry Subcommentlabel = (Entry)ParentStackLayout.Children[0];
            string subcomment = Subcommentlabel.Text;
            Console.WriteLine(subcomment);
            Guid x = Guid.NewGuid();
            if (App.CurrentPost.Comments == null)
            {
                var com = new List<Comm>();
                com.Add(new Comm { CID = x.ToString(), ParentCID = te, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = subcomment });
                App.CurrentPost.Comments = com;
            }
            else
            {
                App.CurrentPost.Comments.Add(new Comm { CID = x.ToString(), ParentCID=te, CommentorEmail = App.cur_user.email, CommentorName = App.cur_user.surname, Comment = subcomment });

            }
            Console.WriteLine("==5==");
            await App.context.SaveAsync(App.CurrentPost);
            comment.Text = "";
            Commentstatus.Text = "";
        }
        async void ShowReplies(object sender, EventArgs e)
        {
        }
    }
}