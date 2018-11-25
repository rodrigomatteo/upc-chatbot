using Upecito.Model.ViewModel;
using System;
using System.Data.SqlClient;
using Upecito.Data.Common;
using Upecito.Data.Implementation;
using Upecito.Data.Interface;

namespace Upecito.Data.MSSQLSERVER.Implementacion
{
    public class CursoData : BaseData, ICursoData
    {
        public CourseByModuleViewModel GetCourseByModuleActive(int IdAlumno)
        {
            CourseByModuleViewModel output = new CourseByModuleViewModel();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    string cmdText = "SELECT A.CODIGOALUMNO, C.CODIGO, C.NOMBRE, C.ACTIVO FROM GSAV.CURSO C " +
		                     "INNER JOIN GSAV.SECCION S ON C.IDCURSO = S.IDCURSO " +
                             "INNER JOIN GSAV.MODULO M ON S.IDMODULO = M.IDMODULO " +
                             "INNER JOIN GSAV.SECCION_ALUMNO SA ON SA.IDSECCION = S.IDSECCION " +
                             "INNER JOIN GSAV.ALUMNO A ON SA.IDALUMNO = A.IDALUMNO " +
                             "WHERE M.VIGENTE = 1 AND A.IDALUMNO = " + IdAlumno;

                    cmd = new SqlCommand(cmdText, cnn);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            output = new CourseByModuleViewModel
                            {
                                CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO")),
                                Codigo = rd.GetString(rd.GetOrdinal("CODIGO")),
                                Nombre = rd.GetString(rd.GetOrdinal("NOMBRE")),
                                Activo = rd.GetBoolean(rd.GetOrdinal("ACTIVO"))
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