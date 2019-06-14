using Amazon.DynamoDBv2.DocumentModel;
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
	public partial class ReportList : ContentPage
    {
        List<Report> a = new List<Report>();

        public ReportList ()
		{
            InitializeComponent ();
            if (App.cur_user_book.stud_teach== "先生")
            {
                NewRep.IsVisible = true;
            }
            BindingContext = Query();
        }
        List<Report> Query()
        {

            QueryFilter filter = new QueryFilter();
            // message partner is App.User_Recepient.Email
            // cur user in from App.cur_user.email
            filter.AddCondition("PosterEmail", QueryOperator.Equal, App.cur_user.email);
            filter.AddCondition("StudentEmail", QueryOperator.Equal, App.User_Recepient.Email);
            var searchm = App.context.FromQueryAsync<Report>(new QueryOperationConfig()
            {
                IndexName = "StudentEmail-PosterEmail-index",
                Filter = filter
            });
            Console.WriteLine("bb items retrieved");
            a=  searchm.GetRemainingAsync().Result;
            return a;
        }
        public async void DetailedReport(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;
            var te = eventargs.Parameter.ToString();
            Report b  = a.Single(r => r.UID == te);
            await Navigation.PushModalAsync(new ReportOne(b));

        }
        async void NewReport(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NewReport());
        }
    }
}