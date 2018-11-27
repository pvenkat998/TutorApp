using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using TutorApp1.Data;
using TutorApp1.iOS.Data;
using UIKit;
using Xamarin.Forms;

[ assembly :Dependency(typeof(SQLite_iOS))]
namespace TutorApp1.iOS.Data
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS() { }
        public SQLite.SQLiteConnection GetConnection()
        {
            var fileName = "Test.db3";
            var documentpath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentpath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);
            var connection = new SQLite.SQLiteConnection(path);
            return connection;
        }
    }
}