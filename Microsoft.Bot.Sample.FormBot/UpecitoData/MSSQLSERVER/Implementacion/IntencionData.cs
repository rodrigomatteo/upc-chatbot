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
        public Intencion BuscarIntencionConsulta(string nombreIntencion)
        {
            var intencion = new Intencion();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_BUSCARINTENCIONCONSULTA, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@P_INTENCION_NOMBRE", nombreIntencion);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        if (rd.Read())
                        {
                            intencion.IdIntencion = rd.GetInt32(rd.GetOrdinal("IDINTENCIONCONSULTA"));
                            intencion.Nombre = rd.GetString(rd.GetOrdinal("INTENCION_BASE"));                          
                        }
                    }
                }
            }
            catch (Exception)
            {
                //LogError(ex);
            }

            return intencion;
        }
    }
}
