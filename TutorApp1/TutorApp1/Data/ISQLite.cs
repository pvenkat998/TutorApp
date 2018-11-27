using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace TutorApp1.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
