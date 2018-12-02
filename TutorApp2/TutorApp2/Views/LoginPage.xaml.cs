using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TutorApp2.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
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
            Entry_Password.Completed += (s, e) => SignIn(s, e);

        }
        
        private void SignIn(object sender, EventArgs e)
        {
            string txtSysLog="";
            var cs ="";
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            MySqlConnection cur = new MySqlConnection("Server=db4free.net;Port=3306;Database=signin;User Id=tutorapp123;Password=12345678;charset=utf8");
            try
            {
                cs = string.Format("Server=db4free.net;Port=3306;database=signin;User Id=tutorapp123;Password=12345678;charset=utf8");
                
                using (var db = new MySqlConnection(cs))
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
            DisplayAlert( cs, "Login", txtSysLog);//do my sql updarte db


        }
        void SignIn2(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);
            if (user.CheckInformation())
            {
                DisplayAlert("Login2", "Login Success2", "Oke2");//do my sql updarte db
            }
            else
            {
                DisplayAlert("Login2", "Login fail2", "whateva2");
            }


        }
    }
}