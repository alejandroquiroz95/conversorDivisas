using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using conversorDivisas.Services;
using conversorDivisas.Droid;
using System.IO;
using SQLite;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]

namespace conversorDivisas.Droid
{
    class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {

            var nombreBD = "Divisas.db3";
            var ruta = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), nombreBD);
            return new SQLiteConnection(ruta);
        }

    }
}