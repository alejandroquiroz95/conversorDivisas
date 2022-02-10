using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using conversorDivisas.UWP;
using Windows.Storage;
using conversorDivisas.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection))]
namespace conversorDivisas.UWP
{
    class DatabaseConnection : IDatabaseConnection
    {

        public SQLiteConnection DbConnection()
        {

            var nombreBD = "Divisas.db3";
            //ruta = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, nombreBD);
            //ruta = Path.Combine(ApplicationData.Current.LocalFolder.Path, nombreBD);
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nombreBD);
            return new SQLiteConnection(ruta);
        }
    }
}
