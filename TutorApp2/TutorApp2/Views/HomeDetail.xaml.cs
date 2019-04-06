using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using SkiaSharp;
using SkiaSharp.Views.Forms;
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
            App.cur_user.email = "admin";

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
            App.searchResponse = search.GetRemainingAsync().Result;
            foreach (var s in App.searchResponse)
            {
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
            int size = App.searchResponse.Count;
            List<ListOfTeachers> listteachlist = new List<ListOfTeachers>();
            System.Diagnostics.Debug.WriteLine("=====GGWP ======== "+size);
            string imgsrc, imgsrc2;
            //ASSIGNING CELLS
            for (int i = 0; i < 10&&i<size; i=i+2)
            {
                imgsrc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.searchResponse[i].email.ToString() + "_dp.jpg");
           
                {
                    if (size - i != 1) {
                        imgsrc2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.searchResponse[i + 1].email.ToString() + "_dp.jpg");

                        listteachlist.Add(new ListOfTeachers
                {
                    email=App.searchResponse[i].email.ToString(),
                    name = App.searchResponse[i].surname.ToString(),
                    karui_major = App.searchResponse[i].gakunen.ToString(),
                    chuugaku_juken = App.searchResponse[i].gender.ToString(),
                    gakunen = App.searchResponse[i].station.ToString(),
                    image = imgsrc,
                    email2 = App.searchResponse[i+1].email.ToString(),
                    name2 = App.searchResponse[i+1].surname.ToString(),
                    karui_major2 = App.searchResponse[i+1].gakunen.ToString(),
                    chuugaku_juken2 = App.searchResponse[i+1].gender.ToString(),
                    gakunen2 = App.searchResponse[i+1].station.ToString(),
                    image2 = imgsrc2,
                    }
                );
                    }
                    else
                    {
                        listteachlist.Add(new ListOfTeachers
                        {
                            email = App.searchResponse[i].email.ToString(),
                            name = App.searchResponse[i].surname.ToString(),
                            karui_major = App.searchResponse[i].gakunen.ToString(),
                            chuugaku_juken = App.searchResponse[i].gender.ToString(),
                            gakunen = App.searchResponse[i].station.ToString(),
                            image = imgsrc
                        }
                    );

                    }
                }
            }
            BindingContext = listteachlist;

            //-----------------------------------FRONTEND-----------------------
            i1.Source = ImageSource.FromResource("TutorApp2.Images.download.png");
            i2.Source = ImageSource.FromResource("TutorApp2.Images.downloadw.png");
            if (bun.IsChecked==false)
            {
                test.Text = "test success";
                listteachlist.Add(new ListOfTeachers
                {
                    email = App.searchResponse[0].email.ToString(),
                    name = App.searchResponse[0].surname.ToString(),
                    karui_major = App.searchResponse[0].gakunen.ToString(),
                    chuugaku_juken = App.searchResponse[0].gender.ToString(),
                    gakunen = App.searchResponse[0].station.ToString()
                });
                BindingContext = listteachlist;
            }

            b1.Source= ImageSource.FromResource("TutorApp2.Images.Searchicon.png");
            b2.Source = ImageSource.FromResource("TutorApp2.Images.Mailicon.png");
            b3.Source = ImageSource.FromResource("TutorApp2.Images.Forumicon.png");
            b4.Source = ImageSource.FromResource("TutorApp2.Images.Profileicon.png");

        }
        void b1c (object sender,EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail());
        }
        void b2c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail2());
        }
        void b3c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Forum());
        }
        void b4c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ProfilePage());
        }
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs arg)
        {

            SKPaintSurfaceEventArgs args = arg as SKPaintSurfaceEventArgs;
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                // Create 300-pixel square centered rectangle
                float x = (info.Width - 300) / 2;
                float y = (info.Height - 300) / 2;
                SKRect rect = new SKRect(x, y, x + 300, y + 300);

                // Create linear gradient from upper-left to lower-right
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(info.Rect.Left, info.Rect.Top),
                                    new SKPoint(info.Rect.Right, info.Rect.Bottom),
                                    new SKColor[] { SKColors.DeepSkyBlue, SKColors.LimeGreen },
                                    new float[] { 0, 1 },
                                    SKShaderTileMode.Repeat);

                canvas.DrawRect(info.Rect, paint);

                // Draw the gradient on the rectangle
            }
        }
  
    private async void OnTapped2(object sender, EventArgs e)
        {
            TappedEventArgs eventargs = e as TappedEventArgs;

            string te = eventargs.Parameter.ToString();
            var action = await DisplayActionSheet("アクション", "戻る", null, "プロフィールをみる", "メッセージする", "通報する");
            if (action== "プロフィールをみる")
            {
                App.tarprof = App.searchResponse.Single(r => r.email == te);
                App.Target_Prof.Email = te;
                await Navigation.PushModalAsync(new ProfilePage());
            }
            if (action == "メッセージする")
            {
                App.User_Recepient.Email = te;
                App.User_Recepient.Username = "vv";

                await Navigation.PushModalAsync(new MessagePageSimple());
            }
            if (action == "通報する")
            {
                await DisplayAlert("通報できた", "通報できた", te);//do my sql updarte db

            }
            Debug.WriteLine("Action: " + action);
        }

    }
}