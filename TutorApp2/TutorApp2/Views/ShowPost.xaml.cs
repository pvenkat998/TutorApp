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
			InitializeComponent ();
		}
        async void Updatecomments(object sender, EventArgs e)
        {
            if (App.CurrentPost.Comments == null)
            {
                var com = new List<List<string>>();
                com.Add(new List<string> { App.cur_user.surname, comment.Text });
                App.CurrentPost.Comments = com;
            }
            else
            {
                App.CurrentPost.Comments.Add(new List<string> { App.cur_user.surname, comment.Text });
            }
            Console.WriteLine(App.CurrentPost.Comments.ToString());
            await App.context.SaveAsync(App.CurrentPost);
        }
	}
}