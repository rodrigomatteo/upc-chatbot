using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upecito.Data.Common
{
    public class Cn
    {
        public static string cnxOracle;
        static Cn()
        {
            cnxOracle = ConfigurationManager.ConnectionStrings["MyConnectionString"].ToString();
        }

        public static OracleConnection OracleCn() => new OracleConnection(cnxOracle);

    }
}
