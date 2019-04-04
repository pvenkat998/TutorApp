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
			InitializeComponent ();
        }
        public async Task QueryAsync(AWSCredentials credentials, RegionEndpoint region)
        {
            var client = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(client);

            var search = context.FromQueryAsync<Post>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("Grade", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "中")

            });

            Console.WriteLine("items retrieved");

            var searchResponse = await search.GetRemainingAsync();
            foreach (var s in searchResponse)
            {
                Console.WriteLine(s.PosterEmail.ToString());
            }
            // searchResponse.ForEach((s) = > {
            // Console.WriteLine(s.ToString());});

        }

        private async void OnTapped3(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
            var action = await DisplayActionSheet("アクション", "戻る", null, "プロフィールをみる", "メッセージする", "通報する");
            if (action == "プロフィールをみる")
            {
                App.tarprof = App.searchResponse.Single(r => r.email == te);
                App.Target_Prof.Email = te;
                await Navigation.PushModalAsync(new ProfilePage());
            }
            if (action == "メッセージする")
            {
                App.User_Recepient.Email = te;
                App.User_Recepient.Username = "vv";

                await Navigation.PushModalAsync(new MessagePageSimple());
            }
            if (action == "通報する")
            {
                await DisplayAlert("通報できた", "通報できた", te);//do my sql updarte db

            }
            Debug.WriteLine("Action: " + action);
        }
    }
}