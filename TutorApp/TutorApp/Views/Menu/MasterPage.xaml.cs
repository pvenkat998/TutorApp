using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp.Views.Menu
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
        public ListView Listview { get { return listview; } }
        public List<MasterMenuItem> items;
		public MasterPage ()
		{
			InitializeComponent ();
            SetItems();

		}
        void SetItems()
        {
            items = new List<MasterMenuitem>();
            items.Add(new.)
        }
	}
}