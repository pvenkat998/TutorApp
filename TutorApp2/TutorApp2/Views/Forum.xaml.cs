using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
            b1.Source = ImageSource.FromResource("TutorApp2.Images.Searchicon.png");
            b2.Source = ImageSource.FromResource("TutorApp2.Images.Mailicon.png");
            b3.Source = ImageSource.FromResource("TutorApp2.Images.Forumicon.png");
            b4.Source = ImageSource.FromResource("TutorApp2.Images.Profileicon.png");
        }

        void b1c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail());
        }
        void b2c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail2());
        }
        void b3c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Forum());
        }
        void b4c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ProfilePage());
        }
        public async Task QueryAsync()
        {
            QueryFilter filter = new QueryFilter();
            if (App.cur_user.grade == "大")
            {
                filter.AddCondition("Grade", QueryOperator.Equal, "小");
                filter.AddCondition("Grade",QueryOperator.Equal, "中");
                filter.AddCondition("Grade",QueryOperator.Equal, "高");
                filter.AddCondition("Grade",QueryOperator.Equal, "大");
            }
            else
            {
                List<string> fl = new List<string>();
                fl.Add("大");
                fl.Add(App.cur_user.grade);
                filter.AddCondition("Grade",QueryOperator.Equal,  fl);

            }
            var search = App.context.FromQueryAsync<Post>(new QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = filter

            });
            var request = new QueryRequest
            {
                TableName = "forum_posts",
                KeyConditionExpression = "Grade = :v_Id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
            {":v_Id", new AttributeValue { S =  "中" }}
        },
            };
            Console.WriteLine("items retrieved");

            var search2 = App.context.QueryAsync<Post>(request);
            var searchResponse = await search2.GetRemainingAsync();
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
            App.CurrentPost = k[0];
            await Navigation.PushModalAsync(new ShowPost());
        }
    }
}