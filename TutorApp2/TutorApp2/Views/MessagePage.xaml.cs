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
        private List<Message> messagelist;

        /*
public MessagePage ()
{
InitializeComponent ();
email.Text =App.User_Recepient.Email ;
}
*/
        public MessagePage()
        {
            messagelist[0] = new Message { Sender = "s", Reciever = "r", text = "mes",TimeStamp = new DateTime(2008, 5, 1, 8, 30, 52), IsIncoming = true };
            messagelist[1] = new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = DateTime.Now, IsIncoming = false };
            InitializeComponent();
            BindingContext = messagelist;
            Title = "#general";


        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
           // MessagesListView.SelectedItem = null;
        }

        void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
         //   MessagesListView.SelectedItem = null;

        }

    }

}