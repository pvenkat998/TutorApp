using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySql.Data.MySqlClient;

namespace TutorApp2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signup : ContentPage
	{
		public Signup ()
		{
			InitializeComponent ();
            Init();
        }
        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;
            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Signup1(s, e);

        }
        private void Signup1(object sender, EventArgs e)
        {
            string txtSysLog = "";
            var cs = "";
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            MySqlConnection db = new MySqlConnection("Server=db4free.net;Port=3306;Database=tutorapp123;User Id=tutorapp123;Password=12345678;charset=utf8");

            try
            {
 
                {
                    db.Open();
                    txtSysLog = "success";
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                txtSysLog = ex.ToString();
            }
            finally
            {
                cur.Close();
            }
            string a = "hi";
            DisplayAlert(cs, a, txtSysLog);//do my sql updarte db


        }
    }
}