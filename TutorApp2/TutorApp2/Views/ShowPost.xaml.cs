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
		public ShowPost ()
		{
            BindingContext = App.CurrentPost;
			InitializeComponent ();
            Console.WriteLine(":===");
            Console.WriteLine("==1=");
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
            if (App.CurrentPost.Comments == null)
            {
                var com = new List<Comm>();
                com.Add(new Comm { Commentor = App.cur_user.surname, Comment = comment.Text });
                App.CurrentPost.Comments = com;
            }
            else
            {
                App.CurrentPost.Comments.Add(new Comm{ Commentor=App.cur_user.surname, Comment=comment.Text });
            }
            await App.context.SaveAsync(App.CurrentPost);
        }
	}
}