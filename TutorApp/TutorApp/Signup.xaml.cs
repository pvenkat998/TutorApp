using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signup : ContentPage
	{
		public Signup ()
		{
			InitializeComponent();              
		}

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void Handle_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Title", "Signnnn","what");
        }
        private void ButtonClicked(object sender, EventArgs e)
        {
            DisplayAlert("Title", "Signnnn", "what");
        }
    }
}