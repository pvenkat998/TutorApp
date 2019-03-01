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
    public partial class MessagePage : ContentPage
	{
        /*
		public MessagePage ()
		{
			InitializeComponent ();
            email.Text =App.User_Recepient.Email ;
        }
        */
        MainChatViewModel vm;
        public MessagePage()
        {
            InitializeComponent();
            Title = "#general";
            BindingContext = vm = new MainChatViewModel();


            vm.Messages.CollectionChanged += (sender, e) =>
            {
                var target = vm.Messages[vm.Messages.Count - 1];
               // MessagesListView.ScrollTo(target, ScrollToPosition.End, true);
            };

        }


        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }
        /*
        void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           // MessagesListView.SelectedItem = null;
        }

        void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
         //   MessagesListView.SelectedItem = null;

        }
        */

    }

}