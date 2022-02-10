using conversorDivisas.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace conversorDivisas.Models
{
    public class ConnectionDB
    {
        public SQLiteConnection _database;
        private static object _collisionLock = new object();
        public ConnectionDB()
        {
            try
            {
                _database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "algo salio mal");
            }
        }

        public int Save(ModelTableRecord table)
        {
            int _id = 0;
            int numReg = 0;
            int nErrores = 0;

            if((_database != null) && (table != null)){
                try{
                    if (table.Id != 0)
                    {
                        lock (_collisionLock)
                        { numReg = _database.Update(table); }
                        if(numReg == 0){
                            nErrores++; 
                        }
                        else{
                            _id = table.Id;
                        }
                    }
                    else
                    {
                        lock (_collisionLock)
                        { numReg = _database.Insert(table); }
                        if(numReg == 0){
                            nErrores++; 
                        }
                        else{
                            _id = table.Id;
                        }
                    }
                }
                catch (Exception ex){
                    Console.WriteLine(ex.Message);
                }
            }

            if (nErrores == 0){
                return _id; 
            }
            else{
                return 0; 
            }
        }

        public void Execute(string query)
        {
            _database.Execute(query);
        }

        public ObservableCollection<ModelTableRecord> GetHistorial(string query)
        {
            ObservableCollection<ModelTableRecord> Historial = new ObservableCollection<ModelTableRecord>(_database.Query<ModelTableRecord>(query));
            return Historial;
        }

        public void CreateTables()
        {
            _database.CreateTable<ModelTableRecord>();
        }
    }
}
