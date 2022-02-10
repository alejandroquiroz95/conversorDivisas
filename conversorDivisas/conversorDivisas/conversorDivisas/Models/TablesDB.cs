using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace conversorDivisas.Models
{
    public class TablesDB
    {
        private int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }
    }
}
