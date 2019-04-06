using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
        {
            App.tarprof = new App.userdata_v1 { };
            App.tarprof.email = "dummy1";
            App.tarprof.password = "nothing here";
            App.tarprof.surname = "kuma";
            App.tarprof.gender = "m";
            App.tarprof.hitokoto = "俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ俺は海賊王になる男だ";
            App.tarprof.hitokoto = "俺は海賊王になる男だ俺は海賊王になる男だ俺";
            App.tarprof.gakunen = "1";
            App.tarprof.karui_major = "文科１";
            App.tarprof.high_school = "la salle";
            App.tarprof.bach_or_mast = "大学";
            App.tarprof.chuugaku_juken = "あり";
            App.tarprof.shidoukanou = "3 years";
            App.tarprof.shidoukeiken = "1 year";
            App.tarprof.station = "茗荷谷駅";
            BindingContext = App.tarprof;

            InitializeComponent();
            w.LowerChild(canvasView);
            image.Source = ImageSource.FromResource("TutorApp2.Images.kuma.jpg");
            image2.Source = ImageSource.FromResource("TutorApp2.Images.download.png");

            // left top   right down padding 
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