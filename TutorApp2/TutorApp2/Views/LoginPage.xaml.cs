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
using MySqlConnector;

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
            string a = "hi";
            string txtSysLog="";
            var cs ="";
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            string ConnectionString = "server=pvenkat998.czpzqegto9at.ap-northeast-1.rds.amazonaws.com; uid =pvenkat998;port=3306;pwd=Asshole!;database=pvenkat998";
            string ConnectionString2 = "Server=db4free.net;Database=tutorapp123;Uid =tutorapp123;Pwd=12345678;";
            MySqlConnection cConn = new MySqlConnection(ConnectionString2);

            try
            {
                cConn.Open();
                cConn.Close();
                txtSysLog = "Connected";
            }
            catch (Exception ex)
            {
                a = "Not Connected ...";
    
                txtSysLog = ex.Message;
            }
            DisplayAlert( cs, txtSysLog, a);//do my sql updarte db


        }
        void Redirsignup(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Signup());
        }
    }
}