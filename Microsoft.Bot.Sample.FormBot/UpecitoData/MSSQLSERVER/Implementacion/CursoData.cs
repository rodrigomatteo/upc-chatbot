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
    public class CursoData : BaseData, ICursoData
    {
        public List<CourseByModuleViewModel> GetCourseByModuleActive(int idAlumno, string curso)
        {
            List<CourseByModuleViewModel> output = new List<CourseByModuleViewModel>();

            try
            {
                using (var cnn = MSSQLSERVERCnx.MSSqlCnx())
                {
                    SqlCommand cmd = null;
                    cnn.Open();

                    cmd = new SqlCommand(SP.GSAV_SP_DOCENTECURSO, cnn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@pIdAlumno", idAlumno);
                    cmd.Parameters.AddWithValue("@pCurso", curso);

                    //string cmdText = "SELECT A.CODIGOALUMNO, C.CODIGO, C.NOMBRE, C.ACTIVO FROM GSAV.CURSO C " +
		                  //   "INNER JOIN GSAV.SECCION S ON C.IDCURSO = S.IDCURSO " +
                    //         "INNER JOIN GSAV.MODULO M ON S.IDMODULO = M.IDMODULO " +
                    //         "INNER JOIN GSAV.SECCION_ALUMNO SA ON SA.IDSECCION = S.IDSECCION " +
                    //         "INNER JOIN GSAV.ALUMNO A ON SA.IDALUMNO = A.IDALUMNO " +
                    //         "WHERE M.VIGENTE = 1 AND A.IDALUMNO = " + IdAlumno;

                    //cmd = new SqlCommand(cmdText, cnn);

                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            output.Add(new CourseByModuleViewModel
                            {
                                CodigoAlumno = rd.GetString(rd.GetOrdinal("CODIGOALUMNO")),
                                Codigo = rd.GetString(rd.GetOrdinal("CODIGO")),
                                Curso = rd.GetString(rd.GetOrdinal("CURSO")),
                                IdCurso = rd.GetInt32(rd.GetOrdinal("IDCURSO")),
                                Activo = rd.GetBoolean(rd.GetOrdinal("ACTIVO")),
                                Seccion = rd.GetString(rd.GetOrdinal("SECCION")),
                                Email = rd.GetString(rd.GetOrdinal("EMAIL"))
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