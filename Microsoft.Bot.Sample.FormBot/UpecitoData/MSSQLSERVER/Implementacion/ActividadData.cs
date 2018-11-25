using Upecito.Model.ViewModel;
using System;
using System.Data.SqlClient;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;

namespace Upecito.Data.MSSQLSERVER.Implementacion
{
    public class ActividadData : BaseData, IActividadData
    {
        public ActivitiesByCourseViewModel GetActivitiesByCourse(int IdAlumno)
        {
            ActivitiesByCourseViewModel output = new ActivitiesByCourseViewModel();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    string cmdText = "SELECT A.CODIGOALUMNO, S.DESCRIPCION, C.NOMBRE ,TA.IDTIPOACTIVIDAD, TA.DESCRIPCION , AC.NUMEROACTIVIDAD " +
                            "FROM [GSAV].[ACTIVIDAD] AC " +
			                "INNER JOIN [GSAV].[TIPO_ACTIVIDAD] TA " +
			                "ON AC.IDTIPOACTIVIDAD = TA.IDTIPOACTIVIDAD " +
                            "INNER JOIN GSAV.SECCION S  " +
                            "ON AC.IDSECCION = S.IDSECCION " +
                            "INNER JOIN GSAV.CURSO C " +
                            "ON S.IDCURSO = C.IDCURSO " +
                            "INNER JOIN GSAV.SECCION_ALUMNO SA ON SA.IDSECCION = S.IDSECCION " +
                            "INNER JOIN GSAV.ALUMNO A ON SA.IDALUMNO = A.IDALUMNO" +
                            "WHERE A.IDALUMNO = " + IdAlumno;

                    cmd = new SqlCommand(cmdText, cnn);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            output = new ActivitiesByCourseViewModel
                            {
                                CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO")),
                                Seccion = rd.GetString(rd.GetOrdinal("SECCION")),
                                Nombre = rd.GetString(rd.GetOrdinal("NOMBRE")),
                                IdTipoActividad = rd.GetInt32(rd.GetOrdinal("IDTIPOACTIVIDAD")),
                                Actividad = rd.GetString(rd.GetOrdinal("ACTIVIDAD")),
                                NumeroActividad = rd.GetInt32(rd.GetOrdinal("NUMEROACTIVIDAD"))
                            };
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return output;
        }
    }
}