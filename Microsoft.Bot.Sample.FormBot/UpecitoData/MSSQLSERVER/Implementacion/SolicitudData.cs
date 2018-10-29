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
    public class SolicitudData : BaseData, ISolicitudData
    {
        public Solicitud Atender(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario)
        {
            var solicitud_ = new Solicitud();
            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_ACTUALIZARSOLICITUD, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pIdSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("@pIdIntencion", idIntencion);
                    cmd.Parameters.AddWithValue("@pSolucion", solucion);
                    cmd.Parameters.AddWithValue("@pEstado", estado);
                    cmd.Parameters.AddWithValue("@pUsuario", usuario);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud_ = new Solicitud();
                            solicitud_.IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD"));
                            solicitud_.IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO"));
                            solicitud_.IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO"));
                            solicitud_.Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                            solicitud_.FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"));
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }

            return solicitud_;
        }

        public Solicitud Crear(int idCanalAtencion, long idAlumno, int? idCurso, string consulta, string usuario)
        {
            var solicitud_ = new Solicitud();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CREARSOLICITUD, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pIdCanalAtencion", idCanalAtencion);
                    cmd.Parameters.AddWithValue("@pIdAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@pIdCurso", idCurso);
                    cmd.Parameters.AddWithValue("@pConsulta", consulta);
                    cmd.Parameters.AddWithValue("@pUsuario", usuario);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud_ = new Solicitud();
                            solicitud_.IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD"));
                            solicitud_.IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO"));
                            solicitud_.IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO"));
                            solicitud_.Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA"));
                            solicitud_.FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"));
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
            return solicitud_;
        }
    }
}
