using FormBot.Util;
using System;
using System.Data;
using System.Data.SqlClient;
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

                    cmd = new SqlCommand(SP.GSAV_SP_ACTUALIZARSOLICITUD, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@pIdSolicitud", idSolicitud);
                    cmd.Parameters.AddWithValue("@pIdIntencion", idIntencion);
                    cmd.Parameters.AddWithValue("@pSolucion", solucion);
                    cmd.Parameters.AddWithValue("@pEstado", estado);
                    cmd.Parameters.AddWithValue("@pUsuario", usuario);
                    cmd.Parameters.AddWithValue("@pFechaActualiza", ConvertidorUtil.GmtToPacific(DateTime.Now));

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud_ = new Solicitud
                            {
                                IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD")),
                                IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO")),
                                IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO")),
                                Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA")),
                                FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"))
                            };
                        }
                    }

                }

            }
            catch (Exception)
            {

            }

            return solicitud_;
        }

        public Solicitud Crear(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario)
        {
            var solicitud_ = new Solicitud();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CREARSOLICITUD, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@pIdCanalAtencion", idCanalAtencion);
                    cmd.Parameters.AddWithValue("@pIdAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@pIdCurso", idCurso);
                    cmd.Parameters.AddWithValue("@pIdSesion", idSesion);
                    cmd.Parameters.AddWithValue("@pConsulta", consulta);
                    cmd.Parameters.AddWithValue("@pUsuario", usuario);
                    cmd.Parameters.AddWithValue("@pFechaCreacion", ConvertidorUtil.GmtToPacific(DateTime.Now));

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud_ = new Solicitud
                            {
                                IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD")),
                                IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO")),
                                IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO")),
                                Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA")),
                                FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO"))
                            };
                        }
                    }

                }

            }
            catch (Exception)
            {

            }
            return solicitud_;
        }
    }
}
