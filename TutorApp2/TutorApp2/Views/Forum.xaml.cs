﻿using Amazon;
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
            App.tarprof.email = App.cur_user.email;
            Navigation.PushModalAsync(new ProfilePage());
        }
        public async Task QueryAsync()
        {
            QueryFilter filter = new QueryFilter();
            QueryFilter filter2 = new QueryFilter();
            QueryFilter filter3 = new QueryFilter();
            QueryFilter filter4 = new QueryFilter();
            if (App.cur_user.grade == "大")
            {
                Console.WriteLine("daaaaaaaaaaaaaaaai");
                filter.AddCondition("Grade", QueryOperator.Equal, "大");
                filter2.AddCondition("Grade", QueryOperator.Equal, "中");
                filter3.AddCondition("Grade", QueryOperator.Equal, "小");
                filter4.AddCondition("Grade", QueryOperator.Equal, "高");
            }
            else
            {
                filter.AddCondition("Grade",QueryOperator.Equal, "大");
                filter2.AddCondition("Grade", QueryOperator.Equal,App.cur_user.grade);
                filter3.AddCondition("Grade", QueryOperator.Equal, "小aweaw");
                filter4.AddCondition("Grade", QueryOperator.Equal, "qweqe高");

            }
            var search = App.context.FromQueryAsync<Post>(new QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = filter
            });
            var search2 = App.context.FromQueryAsync<Post>(new QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = filter2
            });
            var search3 = App.context.FromQueryAsync<Post>(new QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = filter3
            });
            var search4 = App.context.FromQueryAsync<Post>(new QueryOperationConfig()
            {
                IndexName = "Grade-index",
                Filter = filter4
            });
            Console.WriteLine("items retrieved");
            List<Post> searchResponse = await search.GetRemainingAsync();
            List<Post> searchResponse2 = await search2.GetRemainingAsync();
            List<Post> searchResponse3 = await search3.GetRemainingAsync();
            List<Post> searchResponse4 = await search4.GetRemainingAsync();
            List<Post> searchResponseres = searchResponse.Union(searchResponse2).Union(searchResponse3).Union(searchResponse4).ToList();
            App.QueriedPosts = searchResponseres; 
            BindingContext = App.QueriedPosts;
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