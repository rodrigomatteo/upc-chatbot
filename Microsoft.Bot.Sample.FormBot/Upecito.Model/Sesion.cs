using System;
using System.Collections.Generic;
using System.Text;

namespace Upecito.Model
{
    public class Sesion
    {
        public long IdSesion { get; set; }
        public long IdAlumno { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string UserName { get; set; }
    }
}
