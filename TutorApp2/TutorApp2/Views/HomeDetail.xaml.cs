using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeDetail : ContentPage
    {
        public HomeDetail()
        {
            InitializeComponent();
            img.Source = ImageSource.FromResource("TutorApp2.Images.LoginIcon.jpg");
            img.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG");
            txt.Text = "i am a retard";
            txt.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG");


        }
    }
}