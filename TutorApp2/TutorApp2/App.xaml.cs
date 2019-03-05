﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Views;
using Amazon.CognitoIdentity;
using Amazon;
using Amazon.DynamoDBv2.DataModel;
using TutorApp2.Models;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TutorApp2
{
    public partial class App : Application
    {
        public static CognitoAWSCredentials credentials = new CognitoAWSCredentials(
    "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
       RegionEndpoint.APNortheast1 // Region
   );
        public static RegionEndpoint region = RegionEndpoint.APNortheast1;
        [DynamoDBTable("userdata_v1")]
        public class userdata_v1
        {
            [DynamoDBHashKey]    // Hash key.
            public string email { get; set; }
            [DynamoDBRangeKey]
            public string password { get; set; }
            public string stud_teach { get; set; }
            public string surname { get; set; }
            public string gender { get; set; }
            public string age { get; set; }
            public string address { get; set; }
            public string bach_or_mast { get; set; }
            public string gakunen { get; set; }
            public string karui_major { get; set; }
            public string high_school { get; set; }
            public string strong_subject { get; set; }
            public string station { get; set; }
        }
        public static Image dp_img;
        public static string dp_img_path;
        public static List<MessageDynamo> messearchResponse;
        public static class cur_user
        {
            public static string email { get; set; }
            public static string add_ku_sort { get; set; }
            public static int id { get; set; }
            public static string username { get; set; }
            public static string password { get; set; }
            public static string address { get; set; }
        }
        public static class User_Recepient
        {
            public static string Email { get; set; }
            public static string Username { get; set; }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
