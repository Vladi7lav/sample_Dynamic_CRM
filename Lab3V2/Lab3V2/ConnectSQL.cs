using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3V2
{
    class ConnectSQL : IDisposable
    {
        public SqlConnection SqlCon { get; }

        public ConnectSQL(String strCon)
        {
            SqlConnection SqlCon = new SqlConnection(strCon);
            SqlCon.Open();
            
        }

        public void Dispose()
        {
            SqlCon.Close();
        }
    }
}
