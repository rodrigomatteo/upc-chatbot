using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Upecito.Model.ViewModel
{
    public class ActivitiesByCourseViewModel
    {
        public string CodigoAlumno { get; set; }
        public string Seccion { get; set; }
        public string Nombre { get; set; }
        public int IdTipoActividad { get; set; }
        public string Actividad { get; set; }
        public int NumeroActividad { get; set; }

    }
}