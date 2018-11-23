﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp1.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TutorApp1
{
    public partial class App : Application
    {
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
