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
            image.Source = ImageSource.FromResource("TutorApp2.Images.kuma.jpg");
            image2.Source = ImageSource.FromResource("TutorApp2.Images.download.png");
            // left top   right down padding 
        }
    }
}