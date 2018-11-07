using System;
using System.Collections.Generic;
using System.Text;

namespace Upecito.Model
{
    public class Solicitud
    {
        public long IdSolicitud { get; set; }
        public long? IdAlumno { get; set; }
        public int? IdCurso { get; set; }
        public string Consulta { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string LogUsuario { get; set; }
        public DateTime LogFecha { get; set; }
    }
}
