using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySql.Data.MySqlClient;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.CognitoIdentity;
using Amazon;

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
            Entry_Password.Completed += (s, e) => Entry_Email.Focus();
            Entry_Email.Completed += (s, e) => Entry_Address.Focus();
            Entry_Address.Completed += (s, e) => Signup1(s, e);

        }
        [DynamoDBTable("registered_userdata")]
        public class registered_userdata
        {
            [DynamoDBHashKey]    // Hash key.
            public string email { get; set; }
            [DynamoDBRangeKey]
            public string add_ku_sort { get; set; }
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            //  public int Price { get; set; }
            public string address { get; set; }
          //  public string datetime { get; set; }
        }
        private void Signup1(object sender, EventArgs e)
        {
            var title = "登録完了";
            string textbox = "登録完了";
            string button = "はい！";
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            //cred region
            CognitoAWSCredentials credentials = new CognitoAWSCredentials(
             "ap-northeast-1:65003829-3bb8-4228-a97c-559a1b370746", // Identity pool ID
                RegionEndpoint.APNortheast1 // Region
            );
            AWSConfigs.AWSRegion = "APNortheast1";
            RegionEndpoint region = RegionEndpoint.APNortheast1;

            var dbclient = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            DateTime now = DateTime.Now.ToLocalTime();
            string text = now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            registered_userdata tosave_info = new registered_userdata()
            {
                email = Entry_Email.Text,
                add_ku_sort="kanagawa",
                id = 2,
                username = Entry_Username.Text,
                password = Entry_Password.Text,
                address = Entry_Address.Text,
               // datetime =  text

            };
            context.SaveAsync(tosave_info);
            DisplayAlert(title, textbox, button);//do my sql updarte db
            Navigation.PushModalAsync(new LoginPage());


        }
    }
}
