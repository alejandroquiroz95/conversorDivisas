using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace conversorDivisas.Models
{
    [Table("historial")]
    public class TableRecord
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get
            { return id; }
            set
            { this.id = value; }
        }

        private string dolar;
        public string Dolar
        {
            get
            { return dolar; }
            set
            { this.dolar = value; }
        }

        private string euro;
        public string Euro
        {
            get
            { return euro; }
            set
            { this.euro = value; }
        }

        private string fecha;
        public string Fecha
        {
            get
            { return fecha; }
            set
            { this.fecha = value; }
        }
    }
    
}
