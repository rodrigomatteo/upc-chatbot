using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Model;

namespace Upecito.Data.MSSQLSERVER.Implementacion
{
    public class IntencionData : BaseData, IIntencionData
    {
        public Intencion BuscarIntencionConsulta(string intencion)
        {
            var categoria = new Intencion();

            try
            {
                //var cnxOracle = ConfigurationManager.ConnectionStrings[ConnectionName].ToString();

                //using (var oCnn = new OracleConnection(cnxOracle))
                //{
                //    OracleCommand oCmd = null;
                //    oCnn.Open();
                //    oCmd = new OracleCommand("SP_BUSCARINTENCIONCONSULTA", oCnn);
                //    oCmd.CommandType = CommandType.StoredProcedure;
                //    oCmd.Parameters.Add(new OracleParameter("pIntencion", OracleDbType.Varchar2)).Value = intencion;
                //    oCmd.Parameters.Add(new OracleParameter("RESULTADO", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                //    var rd = oCmd.ExecuteReader();
                //    if (rd.HasRows)
                //    {
                //        if (rd.Read())
                //        {
                //            categoria = new Intencion()
                //            {
                //                IdIntencion = rd.GetInt32(rd.GetOrdinal("IDINTENCIONCONSULTA")),
                //                Nombre = rd.GetString(rd.GetOrdinal("NOMBRE"))
                //            };
                //        }
                //    }

                //}

                return categoria;
            }
            catch (Exception)
            {
                //LogError(ex);
            }

            return null;
        }
    }
}
