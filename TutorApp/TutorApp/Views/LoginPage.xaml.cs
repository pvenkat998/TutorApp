using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp.Models;
using TutorApp.Views;

namespace TutorApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}


        void SignIn(object sender , EventArgs e)
        {
            User user = new User(Entry_Username.Text,Entry_Password.Text); 
            if (user.CheckInformation())
            {
                DisplayAlert("Login", "Login Success","Oke");
            }
            else
            {
                DisplayAlert("Login", "Login fail", "empty");
            }
        }

    }
}