using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace conversorDivisas.Models
{
    [Table("historial")]
    public class ModelTableRecord
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

        private string usd;
        public string USD
        {
            get
            { return usd; }
            set
            { this.usd = value; }
        }

        private string eur;
        public string EUR
        {
            get
            { return eur; }
            set
            { this.eur = value; }
        }

        private string mxn;
        public string MXN
        {
            get
            { return mxn; }
            set
            { this.mxn = value; }
        }

        private string gbp;
        public string GBP
        {
            get
            { return gbp; }
            set
            { this.gbp = value; }
        }

        private string cad;
        public string CAD
        {
            get
            { return cad; }
            set
            { this.cad = value; }
        }

        private string aud;
        public string AUD
        {
            get
            { return aud; }
            set
            { this.aud = value; }
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
