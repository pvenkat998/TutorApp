using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TutorApp1.Data;
using TutorApp1.Droid.Data;
using Xamarin.Forms;

[assembly:Dependency(typeof(SQLite_Android))]

namespace TutorApp1.Droid.Data
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqlliteFilename = "Test.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath,sqlliteFilename);
            var conn = new SQLite.SQLiteConnection(path);

            return conn;

        }
    }
}