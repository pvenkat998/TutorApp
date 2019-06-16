using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Drawing;
using Amazon;
using Amazon.S3;
using Amazon.CognitoIdentity;
using System.Net.Http;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using Plugin.CurrentActivity;
using FFImageLoading.Forms.Platform;
namespace TutorApp2.Droid
{
    [Activity(Label = "TutorApp2", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Xamarians.CropImage.Droid.CropImageServiceAndroid.Initialize(this);

            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);
            #region Resolver Init
            SimpleContainer container = new SimpleContainer();
            container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

            Resolver.SetResolver(container.GetResolver());
            #endregion
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }
    }
}