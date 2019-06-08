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
        public SignupTemp()
        {
            InitializeComponent();
        }
        void Signup(object sender, EventArgs e)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("pvenkat998@gmail.com");
                mail.To.Add("animegamingrevolutionary@gmail.com");
                mail.Subject = "ez";
                mail.Body = "why so ez";

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("pvenkat998@gmail.com", "itaminokami");

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                DisplayAlert("Faild", ex.Message, "OK");
            }
            try
            {

                TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                request.BucketName = "tutorapp" + @"/" + "profilepic";
                request.Key = "default.jpg";
                request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg");

                System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                App.s3utility.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
            }
            catch
            {

            }
            App.userdata_v1 tosave_info = new App.userdata_v1()
            {
                email = Email.Text,
                surname = Name.Text,

            };
            App.context.SaveAsync(tosave_info);
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