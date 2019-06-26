using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupTemp : ContentPage
    {
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public SignupTemp()
        {
            InitializeComponent();
        }
        void Signup(object sender, EventArgs e)
        {

            int x = GenerateRandomNo();
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("pahonedu@gmail.com");
                mail.To.Add("animegamingrevolutionary@gmail.com");
                mail.Subject = "Confirmation code ";
                mail.Body = "The code is" +x ;

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("pahonedu@gmail.com", "TutorApp7!");

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                DisplayAlert("Faild", ex.Message, "OK");
            }

            Navigation.PushModalAsync(new SignupTempConfPage(Email.Text, Name.Text,x,Password.Text));

        }
        void Sensei(object sender, EventArgs e)
        {
            Row.IsVisible = false;

        }
        void Stud(object sender,EventArgs e)
        {
            Label.Text = "親はこのアプリ使っていますか？";
            Row.IsVisible = true;
        }
        void Parentt(object sender, EventArgs e)
        {
            Label.Text = "子供はこのアプリ使っていますか？";
            Row.IsVisible = true;

        }
    }
}