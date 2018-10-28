using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Upecito.Data.Common
{
    public class MSSQLSERVERCnx
    {
        public static string cnxMSSQLSERVER;

        static MSSQLSERVERCnx()
        {
            cnxMSSQLSERVER = System.Configuration.ConfigurationManager.ConnectionStrings["GSAV_MSSQLSERVER"].ToString();
        }

        public static SqlConnection MSSqlCnx() => new SqlConnection(cnxMSSQLSERVER);
    }
}
