using Amazon.S3.Transfer;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            if (App.tarprof.email == App.cur_user.email)
            {
                App.tarprof = App.cur_user_book;
            }
            else
            {

            }
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.tarprof.email.ToString() + "_dp.jpg")))
            {
            }
            else
            {
                TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                request.BucketName = "tutorapp" + @"/" + "profilepic";
                request.Key = App.tarprof.email + "_dp.jpg";
                request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.tarprof.email.ToString() + "_dp.jpg");
                try
                {
                    App.s3utility.DownloadAsync(request).ConfigureAwait(true);

                }
                catch
                {
                    Console.WriteLine("===navprof==");
                }
            }

            BindingContext = App.tarprof;

            InitializeComponent();
            //w.LowerChild(canvasView);
            image.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "dummy1@example.com" + "_dp.jpg");
            if (App.tarprof.gender == "男") {
                //image2.Source = ImageSource.FromResource("TutorApp2.Images.male.png");
            }
            else if (App.tarprof.gender == "女")
            {
                // image2.Source = ImageSource.FromResource("TutorApp2.Images.female.png");
            }
            // left top   right down padding 
            b1.Source = ImageSource.FromResource("TutorApp2.Images.Searchicon.png");
            b2.Source = ImageSource.FromResource("TutorApp2.Images.Mailicon.png");
            b3.Source = ImageSource.FromResource("TutorApp2.Images.Forumicon.png");
            b4.Source = ImageSource.FromResource("TutorApp2.Images.Profileicon.png");
        }
        void MsgRdr(object sender, EventArgs e)
        {
            App.User_Recepient.Email = App.tarprof.email;
            App.User_Recepient.Username = App.tarprof.surname;
            App.User_Recepient.Grade = App.tarprof.edu_tier;
            Navigation.PushModalAsync(new MessagePageSimple());
        }
        async void Like(object sender, EventArgs e)
        {
            string x = Guid.NewGuid().ToString();
            MessageDynamo mes = new MessageDynamo
            {
                Messageid = x,
                Sender = App.cur_user.email,
                Reciever = App.tarprof.email,
                Message = x,
                TimeStamp = DateTime.Now
            };
            await App.context.SaveAsync(mes); 
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
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs arg)
        {

            SKPaintSurfaceEventArgs args = arg as SKPaintSurfaceEventArgs;
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                // Create 300-pixel square centered rectangle
                float x = (info.Width - 300) / 2;
                float y = (info.Height - 300) / 2;
                SKRect rect = new SKRect(x, y, x + 300, y + 300);

                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(info.Rect.Left, info.Rect.Top),
                                    new SKPoint(info.Rect.Right, info.Rect.Bottom),
                                    new SKColor[] { SKColors.DeepSkyBlue, SKColors.LimeGreen },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Repeat);

                canvas.DrawRect(info.Rect, paint);

                // Draw the gradient on the rectangle
        }
        }
    }
}