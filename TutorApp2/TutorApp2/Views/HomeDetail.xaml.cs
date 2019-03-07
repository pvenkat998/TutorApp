using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //-----------------------BACKEND--------------------------      
            var client = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(client);

            //download data from query DB
            //DOWNLOAD MESSAGES DATA
            /*
            var searchm = context.FromQueryAsync<MessageDynamo>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "Sender-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("Sender", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, App.cur_user.email)

            });
            Console.WriteLine("items retrieved");
            
            App.messearchResponse = searchm.GetRemainingAsync().Result;
            var searchm2 = context.FromQueryAsync<MessageDynamo>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "Reciever-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("Reciever", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, App.cur_user.email)

            });
            Console.WriteLine("items retrieved");
        
            //download images
            App.messearchResponse2 = searchm2.GetRemainingAsync().Result;
            // QueryAsync1(App.credentials, App.region).ConfigureAwait(true);
            //  var client = new AmazonDynamoDBClient(App.credentials, App.region);
            //   DynamoDBContext context = new DynamoDBContext(client);
                */
            var search = context.FromQueryAsync<App.userdata_v1>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "stud_teach-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("stud_teach", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "先生")

            });
            Console.WriteLine("items retrieved");

            //download images
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
                    request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), s.email.ToString() + "_dp.jpg");
                    TransferUtility tu = new TransferUtility(s3Client);
                    System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                    tu.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("=====ERROR ========");
                }
            }
            /*    ListOfTeachers listteach =();
               // listteach.image.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "1.jpg");
                listteach.name = searchResponse[0].surname.ToString();
                listteach.gakunen = searchResponse[0].gakunen.ToString();
                listteach.kamoku = searchResponse[0].strong_subject.ToString();
                listteach.moyori = searchResponse[0].station.ToString(); 
                */
            int size = searchResponse.Count;
            List<ListOfTeachers> listteachlist = new List<ListOfTeachers>();
            System.Diagnostics.Debug.WriteLine("=====GGWP ======== "+size);
            string imgsrc;
            //ASSIGNING CELLS
            for (int i = 0; i < 10&&i<size; i++)
            {
                imgsrc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), searchResponse[i].email.ToString() + "_dp.jpg");
                {
                    listteachlist.Add(new ListOfTeachers
                {
                    email=searchResponse[i].email.ToString(),
                    name = searchResponse[i].surname.ToString(),
                    gakunen = searchResponse[i].gakunen.ToString(),
                    kamoku = searchResponse[i].strong_subject.ToString(),
                    moyori = searchResponse[i].station.ToString(),
                   // moyori = imgsrc,
                    image = imgsrc,
                }
                );
                }
            }
            BindingContext = listteachlist;

            //-----------------------------------FRONTEND-----------------------
            img.Source = ImageSource.FromResource("TutorApp2.Images.LoginIcon.jpg");
            img.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG");
            img.Source = App.dp_img_path;
            txt.Text = "pro";
            txt.Text = App.cur_user.address;
        }

        private async void OnTapped2(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
            var action = await DisplayActionSheet("アクション", "戻る", null, "プロフィールをみる", "メッセージする", "通報する");
            if (action== "プロフィールをみる")
            {
                App.Target_Prof.Email = te;
                await Navigation.PushModalAsync(new ProfilePage());
            }
            if (action == "メッセージする")
            {
                var tarprof = App.searchResponse.Single(r => r.email == te);
                App.User_Recepient.Email = te;
                App.User_Recepient.Username = "vv";

                await Navigation.PushModalAsync(new MessagePageSimple());
            }
            if (action == "通報する")
            {
                txt.Text = "reported";
                await DisplayAlert("通報できた", "通報できた", te);//do my sql updarte db

            }
            Debug.WriteLine("Action: " + action);
        }

    }
}