using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApp2.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeDetail : ContentPage
    {
        public HomeDetail()
        {
            InitializeComponent();

            // QueryAsync1(App.credentials, App.region).ConfigureAwait(true);
            var client = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(client);
            var search = context.FromQueryAsync<App.userdata_v1>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "stud_teach-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("stud_teach", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "先生")

            });
            Console.WriteLine("items retrieved");


            var searchResponse = search.GetRemainingAsync().Result;
            foreach (var s in searchResponse)
            {
                t1.Text = s.email.ToString();
                Console.WriteLine(s.email.ToString());
                //pic
                var s3Client = new AmazonS3Client(App.credentials, App.region);
                var transferUtility = new TransferUtility(s3Client);
                try
                {

                    TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                    request.BucketName = "tutorapp" + @"/" + "profilepic";
                    request.Key = s.email.ToString() + "_dp.jpg";
                    request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "1.jpg");
                    TransferUtility tu = new TransferUtility(s3Client);
                    System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                    tu.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("=====ERROR ========");
                }
            }
            ListOfTeachers listteach = new ListOfTeachers();
           // listteach.image.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "1.jpg");
            listteach.name = searchResponse[0].surname.ToString();
            listteach.gakunen = searchResponse[0].gakunen.ToString();
            listteach.kamoku = searchResponse[0].strong_subject.ToString();
            listteach.moyori = searchResponse[0].station.ToString();
            BindingContext = listteach;
            img.Source = ImageSource.FromResource("TutorApp2.Images.LoginIcon.jpg");
            img.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG");
            img.Source = App.dp_img_path;
            txt.Text = "i am a retard";
            txt.Text = App.cur_user.address;

        }

    }
}