using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Simple.Data;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using Upecito.Data.Util;
using Upecito.Model;

namespace Upecito.Data.Oracle.Implementacion
{
    public class SesionData : BaseData, ISesionData
    {
        public Sesion Crear(long idAlumno)
        {
            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CREARSESION", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.Parameters.Add(new OracleParameter("pIdAlumno", OracleDbType.Int64)).Value = idAlumno;
                    oCmd.Parameters.Add(new OracleParameter("pRpta", OracleDbType.Int64)).Direction = ParameterDirection.Output;
                    oCmd.ExecuteScalar();

                    var idSesion = oCmd.Parameters["pRpta"].Value;

                    if (idSesion != null)
                    {
                        var sesion = new Sesion()
                        {
                            IdSesion = UtilConverter.ConvertirInt64(idSesion.ToString()),
                            IdAlumno = idAlumno,
                            FechaInicio = DateTime.Now
                        };
                        return sesion;
                    }

                }
            }
            catch (Exception ex)
            {

            }


            //try
            //{
            //    var db = Database.OpenNamedConnection(ConnectionName);

            //    var result = db.SP_CREARSESION(idAlumno);
            //    var gsavSesion = db.GSAV_SESION.Get(result.OutputValues["PRPTA"]);

            //    var sesion = new Sesion()
            //    {
            //        IdSesion = gsavSesion.IDSESION,
            //        IdAlumno = gsavSesion.IDALUMNO,
            //        FechaInicio = gsavSesion.FECHAINICIO
            //    };

            //    return sesion;
            //}
            //catch (Exception ex)
            //{
            //    LogError(ex);
            //}

            return null;
        }

        public Sesion Cerrar(long idSesion)
        {
            try
            {
                using (var oCnn = Cn.OracleCn())
                {
                    OracleCommand oCmd = null;
                    oCnn.Open();
                    oCmd = new OracleCommand("SP_CERRARSESION", oCnn);
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.Parameters.Add(new OracleParameter("pIdSesion", OracleDbType.Int64)).Value = idSesion;
                    oCmd.Parameters.Add(new OracleParameter("pRpta", OracleDbType.Int64)).Direction = ParameterDirection.Output;
                    oCmd.ExecuteScalar();

                    var idAlumno = oCmd.Parameters["pRpta"].Value;

                    if (idAlumno != null)
                    {
                        var sesion = new Sesion()
                        {
                            IdAlumno = UtilConverter.ConvertirInt64(idAlumno.ToString()),
                            IdSesion = idSesion

                        };
                        return sesion;
                    }

                }
            }
            catch (Exception ex)
            {

            }

            //try
            //{
            //    var db = Database.OpenNamedConnection(ConnectionName);

            //    var result = db.SP_CERRARSESION(idSesion);
            //    var gsavSesion = db.GSAV_SESION.Get(result.OutputValues["PRPTA"]);

            //    var sesion = new Sesion()
            //    {
            //        IdSesion = gsavSesion.IDSESION,
            //        IdAlumno = gsavSesion.IDALUMNO,
            //        FechaInicio = gsavSesion.FECHAINICIO
            //    };

            //    return sesion;
            //}
            //catch (Exception ex)
            //{
            //    LogError(ex);
            //}

            return null;
        }
    }
}
