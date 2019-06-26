using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupTempConfPage : ContentPage
    {
        string email;
        string name;
        string code;
        string password;
        public SignupTempConfPage(string email2, string name2,int code2,string password2)
        {
            email = email2;
            name = name2;
            code = code2.ToString();
            password = password2;
            InitializeComponent();
           
        }
        async void Submit(object sender, EventArgs e) {
            if (Code.Text == code) { 
                    App.userdata_v1 tosave_info = new App.userdata_v1()
                    {
                        email = email,
                        surname = name,
                        password=password,

                    };
                    await App.context.SaveAsync(tosave_info);
                    if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg")))
                    {
                    }
                    else
                    {
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
                    }
                    TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();
                    uprequest.BucketName = "tutorapp" + @"/" + "profilepic";
                    uprequest.Key = email + "_dp.jpg"; //file name up in S3
                    uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "default.jpg");
                    try
                    {
                        await App.s3utility.UploadAsync(uprequest);
                    }
                    catch
                    {
                        Console.WriteLine("no pic");
                    }
                    var nextpage = new LoginPage();
                nextpage.Entry_Username.Text = email;
                nextpage.Entry_Password.Text = password;
                    await Navigation.PushModalAsync(nextpage);
            }
            else
            {
                WrongCodeLbl.IsVisible = true;
            }
        }
    }
}