using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Views;
using Amazon.CognitoIdentity;
using Amazon;

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
