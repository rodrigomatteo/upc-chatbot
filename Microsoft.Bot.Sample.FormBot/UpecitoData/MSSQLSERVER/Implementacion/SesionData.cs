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
    public class SesionData : BaseData, ISesionData
    {
        public Sesion Crear(long idAlumno)
        {
            try
            {
                var sesion = new Sesion();

                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CREARSESION, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@pIdAlumno", idAlumno);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        if (rd.Read())
                        {
                            sesion.IdSesion = Convert.ToInt64(rd.GetDecimal(rd.GetOrdinal("ID_SESSION")));
                            sesion.IdAlumno = idAlumno;
                            sesion.FechaInicio = DateTime.Now;
                        }
                    }
                }

                return sesion;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public Sesion Cerrar(long idSesion)
        {
            try
            {
                var sesion = new Sesion();

                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CERRARSESION, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@pIdSesion", idSesion);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        if (rd.Read())
                        {
                            sesion.IdSesion = rd.GetInt32(rd.GetOrdinal("IDSESION"));
                            sesion.IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO"));
                            sesion.FechaInicio = rd.GetDateTime(rd.GetOrdinal("FECHAFIN"));
                        }
                    }
                }
                return sesion;
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
