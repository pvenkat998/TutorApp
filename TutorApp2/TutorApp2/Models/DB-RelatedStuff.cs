using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static TutorApp2.Views.LoginPage;

namespace TutorApp2.Models
{
    class DB_RelatedStuff
    {
        //QUERY THE QB USING INDEX IN THIS CASE PASSWORD-INDEX
 //---------------------------searchResponse is the list of all things queried ,,,, s is each result s == Book class call stuff with s.email
       //AND FILTER
 public async Task QueryAAsync(AWSCredentials credentials, RegionEndpoint region)
        {                   QueryFilter filter = new QueryFilter();
            // message partner is App.User_Recepient.Email
            // cur user in from App.cur_user.email
            filter.AddCondition("Sender", QueryOperator.Equal, App.cur_user.email);
            filter.AddCondition("Reciever", QueryOperator.Equal, App.User_Recepient.Email);
            var searchm = App.context.FromQueryAsync<MessageDynamo>(new QueryOperationConfig()
            {
                IndexName = "Sender-Reciever-index",
                Filter=filter
            });
            Console.WriteLine("bb items retrieved");
            App.messearchResponse = searchm.GetRemainingAsync().Result;
        }
        public async Task QueryAsync(AWSCredentials credentials, RegionEndpoint region)
        {
            var client = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(client);

            var search = context.FromQueryAsync<App.userdata_v1>(new Amazon.DynamoDBv2.DocumentModel.QueryOperationConfig()
            {
                IndexName = "password-index",
                Filter = new Amazon.DynamoDBv2.DocumentModel.QueryFilter("password", Amazon.DynamoDBv2.DocumentModel.QueryOperator.Equal, "asd")

            });

            Console.WriteLine("items retrieved");

            var searchResponse = await search.GetRemainingAsync();
            foreach (var s in searchResponse)
            {
                Console.WriteLine(s.email.ToString());
            }
            // searchResponse.ForEach((s) = > {
            // Console.WriteLine(s.ToString());});

        }
        //---------------define class
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
//---------------------check a specific entry with id (email in this case )
        public async Task QuerywithIDAsync(AWSCredentials credentials, RegionEndpoint region)
        { 
            var dbclient = new AmazonDynamoDBClient(App.credentials, App.region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            App.userdata_v1 retrievedBook = context.LoadAsync<App.userdata_v1>("admin", "kanagawa").Result;
        }

//------------------save a new thing to the db
        public async Task SaveAsync(AWSCredentials credentials, RegionEndpoint region)
        {
            var dbclient = new AmazonDynamoDBClient(credentials, region);
            DynamoDBContext context = new DynamoDBContext(dbclient);
            registered_userdata tosave_info = new registered_userdata()
            {
                email = "",
                add_ku_sort = "kanagawa",
                id = 2,
                username = "",
                password = "",
                address = "",
                // datetime =  text

            };
            await context.SaveAsync(tosave_info);
        }
        ////////////////////////////////////////////////////////        ------------------------------------------------------------------------
        //-----------------------------enter s3=---------------------------------------------=
        string uppath = "";
        public async Task  S3(AWSCredentials credntials,RegionEndpoint region)
        {
            //Enter S3
            AWSConfigsS3.UseSignatureVersion4 = true;
            var s3Client = new AmazonS3Client(App.credentials, App.region);
            var transferUtility = new TransferUtility(s3Client);


            // picture download success
            string textbox = "w";
            try
            {

                TransferUtilityDownloadRequest request = new TransferUtilityDownloadRequest();
                request.BucketName = "tutorapp";
                request.Key = "Capture.PNG";
                request.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG");
                textbox = request.FilePath;
                TransferUtility tu = new TransferUtility(s3Client);
                request.WriteObjectProgressEvent += WriteFileProgress;

                System.Threading.CancellationToken cancellationToken = new System.Threading.CancellationToken();
                tu.DownloadAsync(request, cancellationToken).ConfigureAwait(true);
            }

            catch (Exception ex)
            {
                textbox = "fuck u bik";
                System.Diagnostics.Debug.WriteLine("=====ERROR ========");
            }
            // picture upload trial 

            TransferUtility utility = new TransferUtility(s3Client);
            // making a TransferUtilityUploadRequest instance
            TransferUtilityUploadRequest uprequest = new TransferUtilityUploadRequest();

            // subdirectory and bucket name
            uprequest.BucketName = "tutorapp" + @"/" + "profilepic";

            uprequest.Key = "test"+ "_" + "workz.jpg"; //file name up in S3
            uprequest.FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Capture.PNG"); //local file name
            uprequest.FilePath = uppath;
            utility.UploadAsync(uprequest); //commensing the transfer

        }
        private void WriteFileProgress(object sender, WriteObjectProgressArgs args)
        {
            // show progress
            System.Diagnostics.Debug.WriteLine("=======UpdateFileProgress=======");
        }
    }
}
