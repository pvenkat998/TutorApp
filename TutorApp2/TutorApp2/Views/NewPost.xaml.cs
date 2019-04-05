using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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
	public partial class NewPost : ContentPage
	{
		public NewPost ()
		{
            App.cur_user.email = "ADMIN";
            App.cur_user.surname = "aa";
            App.cur_user.grade = "chuu";

            InitializeComponent ();
		}
        void Back(object sender,EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        async void NewPost1(object sender,EventArgs e)
        {
            Guid x= Guid.NewGuid();
            var com = new List<List<string>>();
            com.Add(new List<string> { });
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
                PostTime=DateTime.Now
            };
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            await context.SaveAsync(NP);
            await Navigation.PushModalAsync(new Forum());

        }

    }
}