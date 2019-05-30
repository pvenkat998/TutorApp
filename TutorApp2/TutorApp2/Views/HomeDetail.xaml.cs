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
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using static TutorApp2.App;
using ImageCircle.Forms.Plugin.Abstractions;

namespace TutorApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeDetail : ContentPage
    {
        List<userdata_v1> FilterListTeach = new List<userdata_v1>();
        List<ListOfTeachers> listteachlist = new List<ListOfTeachers>();

        void FilterOnClick(object sender, EventArgs e)
        {
            FilterListTeach.Clear();
            int count = 0;
            if (man.IsChecked == true)
            {
                count++;
                FilterListTeach.AddRange(searchResponse.Where(x => x.gender == "男").ToList());
            }
            if (woman.IsChecked == true)
            {
                count++;
                FilterListTeach.AddRange(searchResponse.Where(x => x.gender == "女").ToList());
            }
            if (chu.IsChecked == true)
            {
                count++;
                FilterListTeach.AddRange(searchResponse.Where(x => x.chuugaku_juken == "あり").ToList());
            }
            if (ri.IsChecked == true)
            {
                count++;
                FilterListTeach.AddRange(searchResponse.Where(x => x.karui_major == "理").ToList());
            }
            if (bun.IsChecked == true)
            {
                count++;
                FilterListTeach.AddRange(searchResponse.Where(x => x.karui_major == "文").ToList());
            }
            if (count > 1)
            {
                FilterListTeach = FilterListTeach.GroupBy(x => x)
                 .Where(g => g.Count() > 1)
                 .Select(y => y.Key)
                 .ToList();
                //A intersection B or A inter B inter C 
            }
            //if (man.IsChecked == false)
            //{

            //    FilterListTeach.RemoveAll((x => x.gender == "男"));
            //}
            //if (woman.IsChecked == false)
            //{
            //    FilterListTeach.RemoveAll((x => x.gender == "女"));
            //}
            //if (chu.IsChecked == false)
            //{
            //    FilterListTeach.RemoveAll((x => x.chuugaku_juken == "あり"));
            //}
            //if (ri.IsChecked == false)
            //{
            //    FilterListTeach.RemoveAll((x => x.karui_major == "理"));
            //}
            //if (bun.IsChecked == false)
            //{
            //    FilterListTeach.RemoveAll((x => x.karui_major == "文"));
            //}
            //init list of ListOfTeachers
            List<ListOfTeachers> listteachlistsub = new List<ListOfTeachers>();

            int size = FilterListTeach.Count;
            System.Diagnostics.Debug.WriteLine("=====GGWP ======== " + FilterListTeach.Count());
            string imgsrc, imgsrc2;
            for (int i = 0; i < 10 && i < size; i = i + 2)
            {
                imgsrc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FilterListTeach[i].email.ToString() + "_dp.jpg");

                {

                    if (size - i != 1)
                    {
                        imgsrc2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FilterListTeach[i + 1].email.ToString() + "_dp.jpg");
                        listteachlistsub.Add(new ListOfTeachers
                        {
                            email =FilterListTeach[i].email.ToString(),
                            name = FilterListTeach[i].surname.ToString(),
                            karui_major = FilterListTeach[i].gakunen.ToString(),
                            chuugaku_juken = FilterListTeach[i].gender.ToString(),
                            gakunen = FilterListTeach[i].station.ToString(),
                            image = imgsrc,
                            gender ="un",
                            email2 = FilterListTeach[i + 1].email.ToString(),
                            name2 = FilterListTeach[i + 1].surname.ToString(),
                            karui_major2 = FilterListTeach[i + 1].gakunen.ToString(),
                            chuugaku_juken2 = FilterListTeach[i + 1].gender.ToString(),
                            gakunen2 = FilterListTeach[i + 1].station.ToString(),
                            image2 = imgsrc2,
                            gender2 = FilterListTeach[i + 1].gender.ToString(),
                        }
                );
                    }
                    else
                    {
                        listteachlistsub.Add(new ListOfTeachers
                        {
                            email = FilterListTeach[i].email.ToString(),
                            name = FilterListTeach[i].surname.ToString(),
                            karui_major = FilterListTeach[i].gakunen.ToString(),
                            chuugaku_juken = FilterListTeach[i].gender.ToString(),
                            gakunen = FilterListTeach[i].station.ToString(),
                            image = imgsrc,
                            gender = FilterListTeach[i].gender.ToString(),
                        }
                    );

                    }
                }
            }

            listteachlistsub=listteachlistsub.Distinct().ToList();
            pic1.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), listteachlistsub[0].email.ToString() + "_dp.jpg");
            //  listview.ItemsSource = listteachlistsub;
        }
        public HomeDetail()
        {
            InitializeComponent();
            //-----------------------BACKEND--------------------------      
            var client = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(client);

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
                try
                {

                    TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                    request.BucketName = "tutorapp" + @"/" + "profilepic";
                    request.Key = s.email.ToString() + "_dp.jpg";
                    request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), s.email.ToString() + "_dp.jpg");

                    System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                    App.s3utility.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("=====ERROR ========");
                }
            }

            int size = App.searchResponse.Count;
            System.Diagnostics.Debug.WriteLine("=====GGWP ======== " + size);
            string imgsrc, imgsrc2;
            //ASSIGNING CELLS
            for (int i = 0; i < 10 && i < size; i = i + 2)
            {
                imgsrc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.searchResponse[i].email.ToString() + "_dp.jpg");

                {                        

                    if (size - i != 1)
                    {
                        imgsrc2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), App.searchResponse[i + 1].email.ToString() + "_dp.jpg");
                        listteachlist.Add(new ListOfTeachers
                        {
                            email = App.searchResponse[i].email.ToString(),
                            name = App.searchResponse[i].surname.ToString(),
                            karui_major = App.searchResponse[i].gakunen.ToString(),
                            chuugaku_juken = App.searchResponse[i].gender.ToString(),
                            gakunen = App.searchResponse[i].station.ToString(),
                            image = imgsrc,
                            gender = App.searchResponse[i].gender.ToString(),
                            email2 = App.searchResponse[i + 1].email.ToString(),
                            name2 = App.searchResponse[i + 1].surname.ToString(),
                            karui_major2 = App.searchResponse[i + 1].gakunen.ToString(),
                            chuugaku_juken2 = App.searchResponse[i + 1].gender.ToString(),
                            gakunen2 = App.searchResponse[i + 1].station.ToString(),
                            image2 = imgsrc2,
                            gender2 = App.searchResponse[i + 1].gender.ToString(),
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
                            image = imgsrc,
                            gender = App.searchResponse[i].gender.ToString(),
                        }
                    );

                    }
                }
            }
            BindingContext = listteachlist;

            //-----------------------------------FRONTEND-----------------------
            i1.Source = ImageSource.FromResource("TutorApp2.Images.male.png");
            i2.Source = ImageSource.FromResource("TutorApp2.Images.female.png");

            b1.Source = ImageSource.FromResource("TutorApp2.Images.Searchicon.png");
            b2.Source = ImageSource.FromResource("TutorApp2.Images.Mailicon.png");
            b3.Source = ImageSource.FromResource("TutorApp2.Images.Forumicon.png");
            b4.Source = ImageSource.FromResource("TutorApp2.Images.Profileicon.png");
        }
        async void Move(object sender, EventArgs e)
        {
            Console.WriteLine("==move called=");
            var buttonClickHandler = (Button)sender;
            Grid ParentStackLayout = (Grid)buttonClickHandler.Parent;
            CircleImage img = (CircleImage)ParentStackLayout.Children[0];
            //Image img = (Image)((Grid)((ViewCell)listview.Children[0]).Children[0]).Children[0];
            //ListView listview = w.FindByName<ListView>("listview");
            //Image img = ((Image)((Grid)((ViewCell)((DataTemplate)listview.Children[0]).Children[0]).Children[0]).Children[0]);

            Console.WriteLine("==22S called=");
            List<Task> transition = new List<Task>();
            transition.Add(img.TranslateTo(0, img.TranslationY-100,2000));
            await Task.WhenAll(transition);
        }
        void Logout(object sender, EventArgs e)
        {
            var properties = Application.Current.Properties;
            properties["password"] = ""; Application.Current.SavePropertiesAsync();
            Navigation.PushModalAsync(new LoginPage());
        }
        void b1c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new HomeDetail());
        }

        void b2c(object sender, EventArgs e)
        {
            //Navigation.PushPopupAsync(new LoadingPage());
            Navigation.PushModalAsync(new HomeDetail2());
            //Navigation.PopAllPopupAsync();
            Navigation.PushModalAsync(new ForumMessageTabPage());
        }
        void b3c(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Forum());
            Navigation.PushModalAsync(new ForumMessageTabPage());
        }
        void b4c(object sender, EventArgs e)
        {
            App.tarprof.email = App.cur_user.email;
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
            if (action == "プロフィールをみる")
            {
                App.tarprof = App.searchResponse.Single(r => r.email == te);
                await Navigation.PushModalAsync(new ProfilePage());
            }
            if (action == "メッセージする")
            {
                App.User_Recepient.Email = te;

                App.userdata_v1 k = (App.searchResponse.Single(x => x.email == te));
                App.User_Recepient.Username = k.surname;
                App.User_Recepient.Grade = k.edu_tier;

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