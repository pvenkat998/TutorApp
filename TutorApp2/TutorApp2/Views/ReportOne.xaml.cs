using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportOne : ContentPage
	{
		public ReportOne (Report b)
		{
            InitializeComponent ();
            Title.Text = b.Title;
            Cont.Text = b.Content;

        }
	}
}