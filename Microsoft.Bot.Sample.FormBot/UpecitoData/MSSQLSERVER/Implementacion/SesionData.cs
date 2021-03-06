﻿using FormBot.Util;
using System;
using System.Data;
using System.Data.SqlClient;
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
            Sesion sesion = new Sesion();

            try
            {
                DateTime dateTimeNow = ConvertidorUtil.GmtToPacific(DateTime.Now);

                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_CREARSESION, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@pIdAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@pFechaCreacion", dateTimeNow);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        if (rd.Read())
                        {
                            sesion.IdSesion = Convert.ToInt64(rd.GetDecimal(rd.GetOrdinal("ID_SESSION")));
                            sesion.IdAlumno = idAlumno;
                            sesion.Nombre = rd.GetString(rd.GetOrdinal("NOMBRE"));
                            sesion.ApePaterno = rd.GetString(rd.GetOrdinal("APELLIDOPAT"));
                            sesion.ApeMaterno = rd.GetString(rd.GetOrdinal("APELLIDOMAT"));
                            sesion.CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                            sesion.FechaInicio = dateTimeNow;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogError(ex);
            }

            return sesion;
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
                throw;
            }
        }

        public AlumnoUsuarioViewModel LeerUsuario(int idUsuario)
        {
            AlumnoUsuarioViewModel sesion = new AlumnoUsuarioViewModel();

            try
            {
                DateTime dateTimeNow = ConvertidorUtil.GmtToPacific(DateTime.Now);

                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_LEERDATOSUSUARIO, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@pIdUsuario", idUsuario);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        if (rd.Read())
                        {
                            sesion.IdAlumno = rd.GetInt32(rd.GetOrdinal("IDALUMNO"));
                            sesion.CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO"));
                            sesion.Unidad = rd.GetString(rd.GetOrdinal("UNIDAD"));
                            sesion.IdPersona = rd.GetInt32(rd.GetOrdinal("IDPERSONA"));
                            sesion.IdUsuario = rd.GetInt32(rd.GetOrdinal("IDUSUARIO"));
                            sesion.NombreUsuario = rd.GetString(rd.GetOrdinal("NOMBREUSUARIO"));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogError(ex);
            }

            return sesion;
        }

    }
}
