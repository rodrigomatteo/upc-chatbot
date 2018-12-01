using Upecito.Model.ViewModel;
using System;
using System.Data.SqlClient;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;
using System.Collections.Generic;
using System.Data;

namespace Upecito.Data.MSSQLSERVER.Implementacion
{
    public class ActividadData : BaseData, IActividadData
    {
        public List<ActivitiesByCourseViewModel> GetActivitiesByCourse(int idAlumno)
        {
            List<ActivitiesByCourseViewModel> output = new List<ActivitiesByCourseViewModel>();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                   // string cmdText = "SELECT A.CODIGOALUMNO, S.DESCRIPCION, C.NOMBRE, TA.IDTIPOACTIVIDAD, TA.DESCRIPCION, AC.NUMEROACTIVIDAD, AC.FECHAACTIVIDAD " +
                   //         "FROM [GSAV].[ACTIVIDAD] AC " +
			                //"INNER JOIN [GSAV].[TIPO_ACTIVIDAD] TA " +
			                //"ON AC.IDTIPOACTIVIDAD = TA.IDTIPOACTIVIDAD " +
                   //         "INNER JOIN GSAV.SECCION S  " +
                   //         "ON AC.IDSECCION = S.IDSECCION " +
                   //         "INNER JOIN GSAV.CURSO C " +
                   //         "ON S.IDCURSO = C.IDCURSO " +
                   //         "INNER JOIN GSAV.SECCION_ALUMNO SA ON SA.IDSECCION = S.IDSECCION " +
                   //         "INNER JOIN GSAV.ALUMNO A ON SA.IDALUMNO = A.IDALUMNO" +
                   //         "WHERE A.IDALUMNO = " + idAlumno;

                    //cmd = new SqlCommand(cmdText, cnn)
                    //{
                    //    CommandType = CommandType.Text
                    //};

                    cmd = new SqlCommand(SP.GSAV_SP_ACTIVIDADESCURSOALUMNO, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@pIdAlumno", idAlumno);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            output.Add(new ActivitiesByCourseViewModel
                            {
                                CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO")),
                                Seccion = rd.GetString(rd.GetOrdinal("SECCION")),
                                Curso = rd.GetString(rd.GetOrdinal("NOMBRE")),
                                IdTipoActividad = rd.GetInt32(rd.GetOrdinal("IDTIPOACTIVIDAD")),
                                Actividad = rd.GetString(rd.GetOrdinal("DESCRIPCION")),
                                NumeroActividad = rd.GetInt32(rd.GetOrdinal("NUMEROACTIVIDAD")),
                                FechaActividad = rd.GetDateTime(rd.GetOrdinal("FECHAACTIVIDAD")),
                                IdActividad = rd.GetInt32(rd.GetOrdinal("IDACTIVIDAD")),
                            });
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogError(ex);
            }

            return output;
        }
    }
}