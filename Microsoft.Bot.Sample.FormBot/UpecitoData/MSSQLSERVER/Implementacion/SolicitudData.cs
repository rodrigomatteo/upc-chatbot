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
        public Solicitud Atender(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario, int? idCurso, int? idActividad, int? idEmpleado, int? cumpleSLA, DateTime? fechaSolucion)
        {
            var solicitud = new Solicitud();
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
                    cmd.Parameters.AddWithValue("@pIdCurso", idCurso);
                    cmd.Parameters.AddWithValue("@pUsuario", usuario);
                    cmd.Parameters.AddWithValue("@pFechaActualiza", ConvertidorUtil.GmtToPacific(DateTime.Now));

                    if (estado == "D")
                    {
                        cmd.Parameters.AddWithValue("@pFechaDerivacion", ConvertidorUtil.GmtToPacific(DateTime.Now));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@pFechaDerivacion", null);
                    }

                    cmd.Parameters.AddWithValue("@pFechaSolucion", fechaSolucion);
                    cmd.Parameters.AddWithValue("@pIdActividad", idActividad);
                    cmd.Parameters.AddWithValue("@pIdEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@pCumpleSla", cumpleSLA);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud = new Solicitud
                            {
                                IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD")),
                                IdCanalAtencion = rd.GetInt32(rd.GetOrdinal("IDCANALATENCION")),
                                IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO")),
                                IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO")),
                                IdSesion = rd.GetInt32(rd.GetOrdinal("IDSESION")),
                                LogUsuario = rd.GetString(rd.GetOrdinal("LOGUSUARIO")),
                                Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA")),
                                FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO")),
                                Estado = rd.GetString(rd.GetOrdinal("ESTADO"))
                                //IdActividad = rd.GetInt32(rd.GetOrdinal("IDACTIVIDAD")),
                                //IdEmpleado = rd.GetInt32(rd.GetOrdinal("IDEMPLEADO")),
                                //CumpleSLA = rd.GetInt32(rd.GetOrdinal("CUMPLESLA"))
                            };
                        }
                    }

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogError(ex);
            }

            return solicitud;
        }

        public Solicitud Crear(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario)
        {
            var solicitud = new Solicitud();

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
                    //cmd.Parameters.AddWithValue("@pIdActividad", null);
                    //cmd.Parameters.AddWithValue("@pIdEmpleado", null);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud = new Solicitud
                            {
                                IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD")),
                                IdCanalAtencion = rd.GetInt32(rd.GetOrdinal("IDCANALATENCION")),
                                IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO")),
                                IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO")),
                                IdSesion = rd.GetInt32(rd.GetOrdinal("IDSESION")),
                                LogUsuario = rd.GetString(rd.GetOrdinal("LOGUSUARIO")),
                                Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA")),
                                FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO")),
                                Estado = rd.GetString(rd.GetOrdinal("ESTADO"))
                                //IdActividad = null,
                                //IdEmpleado = null,
                                //CumpleSLA = 0
                            };
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LogError(ex);
                //throw ex;
            }

            return solicitud;
        }

        public Solicitud Leer(long idSesion)
        {
            var solicitud = new Solicitud();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_LEERSOLICITUD, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@pIdSesion", idSesion);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud = new Solicitud
                            {
                                IdSolicitud = rd.GetInt32(rd.GetOrdinal("IDSOLICITUD")),
                                IdCanalAtencion = rd.GetInt32(rd.GetOrdinal("IDCANALATENCION")),
                                IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO")),
                                IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO")),
                                IdSesion = rd.GetInt32(rd.GetOrdinal("IDSESION")),
                                LogUsuario = rd.GetString(rd.GetOrdinal("LOGUSUARIO")),
                                Consulta = rd.GetValue(rd.GetOrdinal("CONSULTA")) == DBNull.Value ? string.Empty : rd.GetString(rd.GetOrdinal("CONSULTA")),
                                FechaRegistro = rd.GetDateTime(rd.GetOrdinal("FECHAREGISTRO")),
                                Estado = rd.GetString(rd.GetOrdinal("ESTADO"))
                                //IdActividad = rd.GetInt32(rd.GetOrdinal("IDACTIVIDAD")),
                                //IdEmpleado = rd.GetInt32(rd.GetOrdinal("IDEMPLEADO")),
                                //CumpleSLA = rd.GetInt32(rd.GetOrdinal("CUMPLESLA"))
                            };
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LogError(ex);
                //throw ex;
            }

            return solicitud;
        }

    }
}
