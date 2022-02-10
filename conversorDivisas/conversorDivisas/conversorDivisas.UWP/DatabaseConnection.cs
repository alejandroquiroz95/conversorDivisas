using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using conversorDivisas.UWP;
using Windows.Storage;

namespace conversorDivisas.UWP
{
    class DatabaseConnection
    {

        public SQLiteConnection DbConnection()
        {
            string ruta = "";
            try
            {
                var nombreBD = "Divisas.db3";
                ruta = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, nombreBD);
                //ruta = Path.Combine(ApplicationData.Current.LocalFolder.Path, nombreBD);
                //var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), nombreBD);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("-------------eror: " + ex);
            }
            return new SQLiteConnection(ruta);
        }
    }
}
