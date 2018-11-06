using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Simple.Data;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Model;

namespace Upecito.Data.Oracle.Implementacion
{
    public class SolicitudData : BaseData, ISolicitudData
    {
        public Solicitud Crear(int idCanalAtencion, long idAlumno, int? idCurso, long? idSesion, string consulta, string usuario)
        {

            var solicitud_ = new Solicitud();

            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CREARSOLICITUD", oCnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    oCmd.Parameters.Add(new OracleParameter("pIdCanalAtencion", OracleDbType.Int32)).Value = idCanalAtencion;
                    oCmd.Parameters.Add(new OracleParameter("pIdAlumno", OracleDbType.Int64)).Value = idAlumno;
                    oCmd.Parameters.Add(new OracleParameter("pIdCurso", OracleDbType.Int64)).Value = idCurso;
                    oCmd.Parameters.Add(new OracleParameter("pIdSesion", OracleDbType.Int64)).Value = idSesion;
                    oCmd.Parameters.Add(new OracleParameter("pConsulta", OracleDbType.Varchar2)).Value = consulta;
                    oCmd.Parameters.Add(new OracleParameter("pUsuario", OracleDbType.Varchar2)).Value = usuario;
                    oCmd.Parameters.Add(new OracleParameter("P_RC", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;

                    OracleDataReader rd = oCmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            solicitud_ = new Solicitud
                            {
                                IdSolicitud = rd.GetInt64(rd.GetOrdinal("IDSOLICITUD")),
                                IdAlumno = rd.GetInt64(rd.GetOrdinal("IDALUMNO")),
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

        public Solicitud Atender(long idSolicitud, long? idIntencion, string solucion, string estado, string usuario)
        {
            var solicitud = new Solicitud();
            try
            {
                //var db = Database.OpenNamedConnection(ConnectionName);

                //var result = db.SP_ACTUALIZARSOLICITUD(idSolicitud, idIntencion, solucion, estado, usuario);
                //var gsavSolicitud = db.GSAV_SOLICITUD.Get(result.OutputValues["PRPTA"]);

                //solicitud = new Solicitud()
                //{
                //    IdSolicitud = gsavSolicitud.IDSOLICITUD,
                //    IdAlumno = gsavSolicitud.IDALUMNO,
                //    IdCurso = gsavSolicitud.IDCURSO,
                //    Consulta = gsavSolicitud.CONSULTA,
                //    FechaRegistro = gsavSolicitud.FECHAREGISTRO
                //};

            }
            catch (Exception)
            {
                //LogError(ex);
            }

            return solicitud;
        }
    }
}
