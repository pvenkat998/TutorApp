using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Models
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessagePageSimple : ContentPage
    {
        List<Message> messagelist = new List<Message>();
        public MessagePageSimple ()
        {

            messagelist.Add(new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = new DateTime(2008, 5, 1, 8, 30, 52), IsIncoming = true ,IsOutgoing= false});
            messagelist.Add(new Message { Sender = "s", Reciever = "r", text = "mes", TimeStamp = DateTime.Now, IsIncoming = false, IsOutgoing=true });
            BindingContext = messagelist;
            InitializeComponent ();
		}
        private void SendMessage(object sender, EventArgs e)
        {
        }
	}
}