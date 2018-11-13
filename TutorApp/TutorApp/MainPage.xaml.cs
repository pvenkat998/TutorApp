using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TutorApp
{
    public partial class MainPage : ContentPage
    {
        static string number = new Random().Next(1, 4).ToString();
        public MainPage()
        {
            InitializeComponent();
        }
        async void ButtonClicked(object sender, EventArgs e )
        {
            Button button = sender as Button;
            
            //Game Over
            if (button.Text==number)
            {
                await DisplayAlert("You are trash", "loser", "tryagain");
                number = new Random().Next(1, 4).ToString();
            }
        }
    }
}
