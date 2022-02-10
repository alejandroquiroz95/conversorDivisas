using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace conversorDivisas.Services
{
    public interface IDatabaseConnection
    {
        SQLiteConnection DbConnection();
    }
}
