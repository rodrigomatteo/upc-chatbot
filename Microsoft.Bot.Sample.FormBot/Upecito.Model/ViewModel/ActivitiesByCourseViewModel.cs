using System;

namespace Upecito.Model.ViewModel
{
    public class ActivitiesByCourseViewModel
    {
        public string CodigoAlumno { get; set; }
        public string Seccion { get; set; }
        public string Curso { get; set; }
        public int IdTipoActividad { get; set; }
        public string Actividad { get; set; }
        public int? IdActividad { get; set; }
        public int NumeroActividad { get; set; }
        public DateTime FechaActividad { get; set; }

    }
}