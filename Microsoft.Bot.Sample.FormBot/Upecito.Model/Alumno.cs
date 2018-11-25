using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Upecito.Model
{
    public class Alumno
    {
        public int IdAlumno { get; set; }
        public string CodigoAlumno { get; set; }
        public string Unidad { get; set; }
        public int IdPersona { get; set; }
    }
}