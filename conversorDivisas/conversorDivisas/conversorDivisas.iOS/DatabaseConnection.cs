using conversorDivisas.iOS;
using conversorDivisas.Services;
using Foundation;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace conversorDivisas.iOS
{
    class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            var nombreBD = "Divisas.db3";
            var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), nombreBD);
            return new SQLiteConnection(ruta);
        }
    }
}