using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Forum : ContentPage
	{
		public Forum ()
        {
            App.cur_user.surname = "ore";
            InitializeComponent ();
            QueryAsync();
        }
        public async Task QueryAsync()
        {
            var search = App.context.FromQueryAsync<Post>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("Grade", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "chuu")

            });

            Console.WriteLine("items retrieved");

            var searchResponse = await search.GetRemainingAsync();
            foreach (var s in searchResponse)
            {
                Console.WriteLine(s.PosterEmail.ToString());
            }
            App.QueriedPosts = searchResponse; 
            BindingContext = searchResponse;
            // searchResponse.ForEach((s) = > {
            // Console.WriteLine(s.ToString());});

        }
        private async void AddPost(object sender,EventArgs e)
        {
            await Navigation.PushModalAsync(new NewPost());
        }
        private async void OnTapped3(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
            var k = App.QueriedPosts.Distinct().Where(x=>x.UID==te).ToList();
            var nextpage = new ShowPost();
            App.CurrentPost = k[0];
            nextpage.BindingContext = k[0];
            await Navigation.PushModalAsync(nextpage);
        }
    }
}