using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewReport : ContentPage
	{
		public NewReport ()
		{
			InitializeComponent ();
		}
        async void newrep(object sender, EventArgs e)
        {
            Guid x = Guid.NewGuid();
            Report rep = new Report()
            {
                UID = x.ToString(),
                Title = Title.Text,
                Content = Contents.Text,
                PostTime = DateTime.Now,
                PostType = "Report",
                PosterEmail = App.cur_user.email,
                PosterName = App.cur_user.surname,
                Grade = App.cur_user.grade
            };
            await App.context.SaveAsync(rep);
        }
	}
}